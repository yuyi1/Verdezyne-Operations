using Operations.Contracts;
using Operations.ControllerHelpers;
using Operations.Repository;
using Operations.Utility;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Operations.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Operations.App_Start.NinjectWebCommon), "Stop")]

namespace Operations.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IRunRepository>().To<RunRepository>();
            kernel.Bind<IDashboardRepository>().To<DashboardRepository>();
            kernel.Bind<IQCRepository>().To<QcRepository>();
            kernel.Bind<IRunHistoryRepository>().To<RunHistoryRepository>();
            kernel.Bind<IReadingSessionRepository>().To<ReadingSessionRepository>();
            kernel.Bind<IReadingRepository>().To<ReadingRepository>();
            kernel.Bind<IEftCalculator>().To<IEftCalculator>();
            kernel.Bind<IRedirectStringBuilderFactory>().To<RedirectStringBuilderFactory>();
            kernel.Bind<IRedirectStringBuilder>().To<RedirectStringBuilder>();
            kernel.Bind<IEftCalculator>().To<EftCalculator>();
            kernel.Bind<IOutlineItemRepository>().To<OutlineItemRepository>();
            kernel.Bind<IOutlineDescriptionRepository>().To<OutlineDescriptionRepository>();
            kernel.Bind<IOutlineTopicRepository>().To<OutlineTopicRepository>();
            kernel.Bind<ISamplePlanRepository>().To<SamplePlanRepository>();
            kernel.Bind<ISamplePlanDetailsRepository>().To<SamplePlanDetailsRepository>();
            kernel.Bind<IMachineRepository>().To<MachineRepository>();
            kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<ITankRepository>().To<TankRepository>();
            kernel.Bind<ICalibrationStandardRepository>().To<CalibrationStandardRepository>();
            kernel.Bind<IStdAnionRepository>().To<StdAnionRepository>();
            kernel.Bind<IStdCationRepository>().To<StdCationRepository>();
            kernel.Bind<IStdIronRepository>().To<StdIronRepository>();
            kernel.Bind<ISampleIronRepository>().To<SampleIronRepository>();
            kernel.Bind<ISampleAnionRepository>().To<SampleAnionRepository>();
            kernel.Bind<ISampleCationRepository>().To<SampleCationRepository>();
                                                   
}        
    }
}
