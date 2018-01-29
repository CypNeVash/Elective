using BusinessModel;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using Ninject.Web.Mvc;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Elective
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DependencyResolver.Current.GetService<IDefaultRepository<Log>>().Add(
                new Log(GetType().ToString()
                , "OnApplicationStarted"
                ,LogStatus.info
                ,"App Start"
                ,""));
        }

        protected override IKernel CreateKernel()
        {
            
            var kernel = new StandardKernel();

            RegisterServices(kernel);
            DependencyResolver.SetResolver(new
                NinjectDependencyResolver(kernel));
            return kernel;
        }
        private void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ElectiveContext>().ToSelf().InRequestScope();
            kernel.Bind<IDefaultRepository<Student>>().To<StudentRepository>();
            kernel.Bind<IDefaultRepository<Teacher>>().To<TeacherRepository>();
            kernel.Bind<IDefaultRepository<Account>>().To<AccountRepository>();
            kernel.Bind<IDefaultRepository<Log>>().To<LogRepository>();
            kernel.Bind<IDefaultRepository<ReportBook>>().To<ReportBookRepository>();
            kernel.Bind<IDefaultRepository<Facultative>>().To<FacultativeRepository>();
            kernel.Bind<IDefaultRepository<Report>>().To<ReportRepository>();
            kernel.Bind<IStudentService>().To<StudentService>();
            kernel.Bind<ITeacherService>().To<TeacherService>();
            kernel.Bind<IAccountService>().To<AccountService>();
        }
    }
}
