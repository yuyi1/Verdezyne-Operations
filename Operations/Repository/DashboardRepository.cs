using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Operations.Contracts;
using Operations.Models;
using Operations.ViewModels;


namespace Operations.Repository
{
    public class DashboardRepository : GenericPilotPlantRepository<VmDashboard>, IDashboardRepository
    {
        private readonly PilotPlantEntities _dbContext;
        private readonly AnalyticsEntities _AnalyticsEntities;

        public DashboardRepository(PilotPlantEntities dbContext, AnalyticsEntities dbAnalyticsEntities)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _AnalyticsEntities = dbAnalyticsEntities;
        }

        // Composes the dashboard model from the various tables
        public VmDashboard GetVmDashboard()
        {
            var vmruns = new RunRepository(_dbContext).GetRunsDto();
            var vmqc = new QcRepository(_AnalyticsEntities).GetRunningQCs();
            var vmseq = new SequenceRepository(_AnalyticsEntities).GetRunningSequences();

            var model = new VmDashboard();
            model.Runs = vmruns.Runs.Where(rm => rm.RunStatusId > 4 & rm.RunStatusId < 11).AsQueryable();
            model.Machines = vmruns.Machines.AsQueryable();
            model.GrowthPhases = vmruns.GrowthPhases;
            model.Products = vmruns.Products;
            model.RunStatus = vmruns.RunStatus;
            model.Readings = vmruns.Readings.Any() ? vmruns.Readings : null;
            model.QC = vmqc;
            model.Sequences = vmseq;

            return model;
        }
    }
}