using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OfficeHandler.FileIO
{
    public class SampleSubmissionPlan
    {
        //S:\Software\Test
        private const string SampleSubmissionMasterPartialFilename = @"Sample Submission V*.xlsx";
        private const string SampleSubmissionMasterStartingPath = @"\\\\james\\Shared\\Research\\Analytical\\Sample Submission";
        //private const string SampleSubmissionMasterStartingPath = @"\\\\james\\Shared\\Software\\Test";

        public static FileInfo CopySubmissionMasterToRun(FileInfo wordFileInfo, string userid)
        {
            FileInfo master = GetFileInfoForMasterSampleSubmissionFile();
            if (wordFileInfo.DirectoryName != null)
            {
                string path = wordFileInfo.DirectoryName;
                string filename = wordFileInfo.Name.Replace("Outline", "Sample_Submission").Replace(".docx", "_Nick.xlsx");
                if (!path.EndsWith("\\"))
                    path += "\\";
                FileInfo dest = new FileInfo(path + filename);
                
                //System.Diagnostics.Debug.WriteLine(dest.FullName);

                if (!dest.Exists)
                {
                    try
                    {
                        File.Copy(master.FullName, dest.FullName);
                        dest.IsReadOnly = false;

                    }
                    catch (Exception ex)
                    {
                        
                        throw new Exception(ex.Message + ex.StackTrace, ex);
                    }
                }
                return dest;
            }
            throw new FileNotFoundException("Could not find the Word File Directory");
        }

        #region "File Handlers"
        /// <summary>
        /// Gets a FileInfo on the Master Sample Submission Excel file from James
        /// </summary>
        /// <returns>FileInfo - on the file</returns>
        public static FileInfo GetFileInfoForMasterSampleSubmissionFile()
        {

            FileInfo master = null;
            FileFinder masterlocater = new FileFinder();
            masterlocater._basefolder = SampleSubmissionMasterStartingPath;
            string sampleSubmissionMasterFilename;
            sampleSubmissionMasterFilename =
                masterlocater.FindByPartialFilename(SampleSubmissionMasterPartialFilename).FirstOrDefault();
            if (sampleSubmissionMasterFilename != null)
            {
                master = new FileInfo(sampleSubmissionMasterFilename);
            }
            else
            {
                throw new FileNotFoundException(string.Format("Cannot find the master Sample Submission Excel file in {0}",
                    SampleSubmissionMasterStartingPath));
            }
            return master;
        }
        /// <summary>
        /// Searches James for a FileInfo for the filename starting at the (Optional) folder
        /// </summary>
        /// <param name="filename">string - the name of the Fermentation Outline file</param>
        /// <param name="startingfolder">(Optional) string - the name of the starting search folder</param>
        /// <returns>FileInfo - on the file</returns>
        public static FileInfo FindWordFileInfo(string filename, ref string startingfolder)
        {
            FileInfo info = null;
            var fermentationfolder = @"\\\\james\\Shared\\Research\\Fermentation\\";
            if (startingfolder.Length > 0)
            {
                fermentationfolder = startingfolder.Replace("S:\\", "\\\\james\\Shared\\");
                if (!fermentationfolder.EndsWith("\\"))
                    fermentationfolder += "\\";
                startingfolder = fermentationfolder;
                string sourceFileName = startingfolder + filename;

                // Get the info on the source file
                //  If it does not exist try to find it starting at the path provided by the user.
                info = new FileInfo(sourceFileName);
                if (!info.Exists)
                {
                    info = UseFileFinderToGetFileInfo(filename, fermentationfolder);
                }
            }
            else
            {
                info = UseFileFinderToGetFileInfo(filename, fermentationfolder);
            }
            return info;
        }
        /// <summary>
        /// Gets a FileInfo for a file being searched for
        /// </summary>
        /// <param name="filename">string - the file name</param>
        /// <param name="startingfolder">string - the starting folder for the search</param>
        /// <returns></returns>
        public static FileInfo UseFileFinderToGetFileInfo(string filename, string startingfolder)
        {
            FileFinder ff = new FileFinder { _basefolder = startingfolder };
            IQueryable<string> file = ff.FindFileByName(filename);
            string sourceFileName = file.FirstOrDefault();
            if (sourceFileName == null)
                throw new FileNotFoundException(string.Format("Unable to find {0} in the Fermentation folder on James",
                    filename));
            FileInfo info = new FileInfo(sourceFileName);
            return info;
        }
        #endregion

    }
}
