using System;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.ApplicationServices;
using System.Web.UI.WebControls;
using Ninject.Activation;
using OfficeHandler;
using OfficeHandler.Excel;
using OfficeHandler.Word.Tables;
using Operations.Contracts;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Repository
{
    public class OutlineItemRepository : GenericPilotPlantRepository<OutlineItem>, IOutlineItemRepository
    {
        private IRunRepository _runRepository;
        private IProductRepository _productRepository;
        private readonly IOutlineDescriptionRepository _outlineDescriptionRepository;
        private IOutlineTopicRepository _outlineTopicRepository;
        private readonly ITankRepository _tankRepository;
        private readonly ISamplePlanRepository _samplePlanRepository;



        // Ninject Constructor injection with various repos.  Ninject handles the lifecycle, so no need to call dispose.
        public OutlineItemRepository(PilotPlantEntities dbContext,
            RunRepository runRepository,
            ProductRepository productRepository,
            OutlineDescriptionRepository outlineDescriptionRepository,
            OutlineTopicRepository outlineTopicRepository,
            TankRepository tankRepository,
            SamplePlanRepository samplePlanRepository)
            : base(dbContext)
        {
            _runRepository = runRepository;
            _productRepository = productRepository;
            _outlineDescriptionRepository = outlineDescriptionRepository;
            _outlineTopicRepository = outlineTopicRepository;
            _tankRepository = tankRepository;
            _samplePlanRepository = samplePlanRepository;
        }
        public OutlineItemsDto GetDtoForRunIdAsync(int runid)
        {
            Run run = DbContext.Runs.FirstOrDefault(e => e.Id == runid);
            if (run == null)
                throw new ObjectNotFoundException(string.Format("Run with Id of {0} not found.", runid));
            var model = new Operations.DTOs.OutlineItemsDto
            {
                Run = run,
                Rundate = run.Rundate,
                Machines = DbContext.Machines.AsQueryable(),
                Tanks = DbContext.Tanks.AsQueryable(),
                OutlineTopics = DbContext.OutlineTopics.AsQueryable(),
                OutlineDescriptions = DbContext.OutlineDescriptions.AsQueryable()
            };

            return model;
        }
        /// <summary>
        /// Persists The Fermentation Outline Word Document's Fermentation Outline Table and Sampling Plan Table to the DB
        /// </summary>
        /// <param name="filename">string - the name of the Word file</param>
        /// <param name="startingfolder">string - an optional starting folder which helps speed search</param>
        /// <param name="productid">string - the id of the product from the UI</param>
        /// <param name="userid">string - the logged in username from the Verdezyne domain.</param>
        /// <returns>bool - true if ok</returns>
        public async Task<Run> PersistWordFile(string filename, string startingfolder, string productid, string machineid, string userid)
        {
            var fileInfo = OfficeHandler.FileIO.SampleSubmissionPlan.FindWordFileInfo(filename, ref startingfolder);
            // Read the Ferm Outline as a Table from the Word File
            if (!fileInfo.Exists)
                throw new FileNotFoundException(string.Format("The file could not be loaded.<table>" +
                                                              "<tr>" +
                                                              "<td colspan=2>" +
                                                              "Check your file on James.</td>" +
                                                              "</tr>" +
                                                              "<td>" +
                                                              "Folder Name:</td><td> {0}</td></tr>" +
                                                              "<tr><td>File Name:</td><td>{1}</td></tr>" +
                                                              "<tr><td colspan=2>If you did not supply a starting folder, the search for the file might have failed." +
                                                              "</td></tr>" +
                                                              "</table>", startingfolder, filename));
            var reader = new FermentationOutlineTable();
            var dataTable = reader.FindTable(fileInfo.FullName);
            if (dataTable == null)
                throw new NullReferenceException(string.Format("There was no Fermentation Outline Table in {0}", filename));

            // Get the name of the machine from column 3 (1 based)
            var machinename = dataTable.Columns[2].ColumnName;

            // Get the Machine Id based on the machine name.
            int lookupmachineid = 0;
            try
            {
                lookupmachineid = new MachineRepository(new PilotPlantEntities()).GetMachineIdFromName(machinename);
                var m = int.Parse(machineid);
                if (m != lookupmachineid)
                {
                    throw new InvalidDataException(
                        "The Machine specified in the form does not agree with the machine name in the Word file");
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex);
            }

            // Get the rundate from the table and Update/Insert Run
            var rundate = dataTable.Rows[1].ItemArray[2].ToString(); // From the table
            var run = await RunInsertUpdateAsync(filename, productid, userid, rundate, lookupmachineid);


            var outlineItems = await OutlineItemsInsertUpdateAsync(userid, lookupmachineid, dataTable, run);
            var samplingPlan = await _samplePlanRepository.SaveTrackingDataAsync(run.Id, fileInfo, userid);



            return run;
        }




        /// <summary>
        /// Inserts an OutlineItem for each Tank in the run based upon the names in the OutlineDescription Table 
        /// </summary>
        /// <param name="userid">string - the logged in user</param>
        /// <param name="machineid">int - the id of the machine being set up</param>
        /// <param name="dataTable">DataTable - As read from the Word Fermentation Outline file for the Run</param>
        /// <param name="run">Run - The Run being set up</param>
        /// <returns>bool - true if ok</returns>
        private async Task<bool> OutlineItemsInsertUpdateAsync(string userid, int machineid, DataTable dataTable, Run run)
        {
            var tanks = _tankRepository.FindTanksByMachine(machineid);
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                // Get the outline description for the Id and the OutlineTopicId
                OutlineDescription od = _outlineDescriptionRepository.FindByName(row.ItemArray[1].ToString());

                // Blank lines are ignored
                if (od != null)
                {
                    foreach (Tank tank in tanks)
                    {
                        try
                        {
                            var outlineItem = new OutlineItem
                            {
                                RunId = run.Id,
                                TankId = tank.Id,
                                OutlineTopicId = od.OutlineTopicId,
                                OutlineDescriptionId = od.Id,
                                PersonId = 0,
                                Value = row.ItemArray[2].ToString(),
                                Checked = false,
                                ValuesState = 0,
                                LastUpdate = DateTime.Now,
                                UpdateBy = userid
                            };
                            var retaddoutlineitem = await UpdateOrAddOutlineItemAsync(outlineItem);

                        }
                        catch (Exception ex)
                        {

                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// // Create a run from the Word Outline file.
        /// </summary>
        /// <param name="filename">string - the name of the document</param>
        /// <param name="productid">string - the product id as a string</param>
        /// <param name="userid">string - the logged in user</param>
        /// <param name="rundate">string - the rundate of the job</param>
        /// <param name="machineid">strng - the id of the machine where the job is being run.</param>
        /// <returns>Run - The inserted Run</returns>
        private async Task<Run> RunInsertUpdateAsync(string filename, string productid, string userid, string rundate, int machineid)
        {
            Run run = new Run
            {
                Name = filename,
                Rundate = rundate,
                RunStart = DateTime.Parse("1980-1-1"),
                InductionStart = DateTime.Parse("1980-1-1"),
                MachineId = machineid,
                ProductId = int.Parse(productid),
                RunStatusId = 5,
                GrowthPhaseId = 1,
                Comments = string.Format("Run Generated from {0}", filename),
                UpdateBy = userid,
                LastUpdate = DateTime.Now
            };
            var runadd = await _runRepository.UpdateOrAddRunAsync(rundate, run);
            return runadd;
        }

        /// <summary>
        /// Adds a single OutlineItem to the DbContext, or updates it if it exists
        /// </summary>
        /// <param name="oi">OutlineItem - the item to be added</param>
        /// <returns>int - the result of the AddAsync or UpdateAsync in the parent class</returns>
        public async Task<int> UpdateOrAddOutlineItemAsync(OutlineItem oi)
        {
            OutlineItem item = null;
            try
            {
                item = DbContext.OutlineItems.FirstOrDefault(r => r.RunId == oi.RunId && r.TankId == oi.TankId && r.OutlineDescriptionId == oi.OutlineDescriptionId);

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                throw;
            }
            if (item != null)
            {
                item.RunId = oi.RunId;
                item.TankId = oi.TankId;
                item.OutlineTopicId = oi.OutlineTopicId;
                item.OutlineDescriptionId = oi.OutlineDescriptionId;
                item.PersonId = oi.PersonId;
                item.Value = oi.Value;
                item.Checked = oi.Checked;
                item.ValuesState = oi.ValuesState;
                item.UpdateBy = oi.UpdateBy;
                item.LastUpdate = DateTime.Now;
                return await UpdateAsync(item);
            }
            else
            {
                return await AddAsync(oi);
            }

        }
    }
}