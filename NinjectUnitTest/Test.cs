using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Elective;
using Ninject;
using System.Web.Mvc;
using Ninject.Web.Mvc;
using BusinessModel;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.WebPages;
using System.Collections.Specialized;
using System.Globalization;

namespace UnitTest
{
    [TestClass]
    public class Test
    {
        private List<User> _users = new List<User>();
        private List<Account> _accounts = new List<Account>();
        private List<Log> _logs = new List<Log>();
        private List<Facultative> _facultatives = new List<Facultative>();
        private List<Student> _students = new List<Student>();
        private List<ReportBook> _reportBooks = new List<ReportBook>();
        private List<Report> _reports = new List<Report>();
        private List<Teacher> _teachers = new List<Teacher>();

        private Mock<ElectiveContext> mockContext;

        [TestInitialize]
        public void MyTestInitialize()
        {
            mockContext = new Mock<ElectiveContext>();
            InitContext();
            InitData();
            RegisterServices();
        }

        private void RegisterServices()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IDefaultRepository<Student>>().To<StudentRepository>()
                .WithConstructorArgument("electiveContext", mockContext.Object);
            kernel.Bind<IDefaultRepository<Teacher>>().To<TeacherRepository>()
                .WithConstructorArgument("electiveContext", mockContext.Object);
            kernel.Bind<IDefaultRepository<Account>>().To<AccountRepository>()
                .WithConstructorArgument("electiveContext", mockContext.Object);
            kernel.Bind<IDefaultRepository<Log>>().To<LogRepository>()
                .WithConstructorArgument("electiveContext", mockContext.Object);
            kernel.Bind<IDefaultRepository<ReportBook>>().To<ReportBookRepository>()
                .WithConstructorArgument("electiveContext", mockContext.Object);
            kernel.Bind<IDefaultRepository<Facultative>>().To<FacultativeRepository>()
                .WithConstructorArgument("electiveContext", mockContext.Object);
            kernel.Bind<IDefaultRepository<Report>>().To<ReportRepository>()
                .WithConstructorArgument("electiveContext", mockContext.Object);

            kernel.Bind<IStudentService>().To<StudentService>();
            kernel.Bind<ITeacherService>().To<TeacherService>();
            kernel.Bind<IAccountService>().To<AccountService>();

            DependencyResolver.SetResolver(new
                NinjectDependencyResolver(kernel));
        }

        private void InitContext()
        {
            var mockSet_accounts = CreateMockSet(_accounts.Union(_teachers)
                .Union(_students).AsQueryable());

            var mockSet_users = CreateMockSet(_users
                .Union(_students.Select(s => s.Identity))
                .Union(_teachers.Select(s => s.Identity)).AsQueryable());

            var mockSet_logs = CreateMockSet(_logs.AsQueryable());
            var mockSet_facultative = CreateMockSet(_facultatives.AsQueryable());
            var mockSet_teacher = CreateMockSet(_teachers.AsQueryable());
            var mockSet_report = CreateMockSet(_reports.AsQueryable());
            var mockSet_reportBook = CreateMockSet(_reportBooks.Union(_facultatives.Where(s => s.Log != null).Select(s => s.Log)).AsQueryable());
            var mockSet_student = CreateMockSet(_students.AsQueryable());

            mockContext.Setup(c => c.Accounts).Returns(mockSet_accounts.Object);
            mockContext.Setup(c => c.Users).Returns(mockSet_users.Object);
            mockContext.Setup(c => c.Students).Returns(mockSet_student.Object);
            mockContext.Setup(c => c.Teachers).Returns(mockSet_teacher.Object);
            mockContext.Setup(c => c.Facultatives).Returns(mockSet_facultative.Object);
            mockContext.Setup(c => c.Reports).Returns(mockSet_report.Object);
            mockContext.Setup(c => c.ReportBooks).Returns(mockSet_reportBook.Object);
            mockContext.Setup(c => c.Logs).Returns(mockSet_logs.Object);

            mockContext.Setup(c => c.Set<Account>()).Returns(mockContext.Object.Accounts);
            mockContext.Setup(c => c.Set<Log>()).Returns(mockContext.Object.Logs);
            mockContext.Setup(c => c.Set<Teacher>()).Returns(mockContext.Object.Teachers);
            mockContext.Setup(c => c.Set<Student>()).Returns(mockContext.Object.Students);
            mockContext.Setup(c => c.Set<User>()).Returns((DbSet<User>)mockContext.Object.Users);
            mockContext.Setup(c => c.Set<Facultative>()).Returns(mockContext.Object.Facultatives);
            mockContext.Setup(c => c.Set<Report>()).Returns(mockContext.Object.Reports);
            mockContext.Setup(c => c.Set<ReportBook>()).Returns(mockContext.Object.ReportBooks);
            mockContext.Setup(c => c.SetModified(It.IsAny<object>())).Callback((object s) => { });

        }

        private Mock<DbSet<T>> CreateMockSet<T>(IQueryable<T> dataQ) where T : class, new()
        {
            Mock<DbSet<T>> mockSet = new Mock<DbSet<T>>();

            object temp = new T();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(dataQ.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(dataQ.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(dataQ.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(dataQ.GetEnumerator());

            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);

            switch (temp)
            {
                case User us:

                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _users.Add(de as User));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _users.Remove(de as User));
                    break;

                case Student stud:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _students.Add(de as Student));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _students.Remove(de as Student));
                    break;

                case Log log:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _logs.Add(de as Log));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _logs.Remove(de as Log));
                    break;

                case Teacher teacher:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _teachers.Add(de as Teacher));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _teachers.Remove(de as Teacher));
                    break;

                case Report rep:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _reports.Add(de as Report));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _reports.Remove(de as Report));
                    break;

                case ReportBook repB:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _reportBooks.Add(de as ReportBook));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _reportBooks.Remove(de as ReportBook));
                    break;

                case Facultative cours:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _facultatives.Add(de as Facultative));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _facultatives.Remove(de as Facultative));
                    break;

                case Account acc:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _accounts.Add(de as Account));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _accounts.Remove(de as Account));
                    break;
            }

            return mockSet;
        }

        private void InitData()
        {
            User user = new User() { UserName = "IvanNagibator007", Email = "IvanNagibator007" };
            Teacher account = new Teacher()
            {
                FirstName = "Ivan"
                ,
                SecondName = "Ivanov"
                ,
                Age = 22
                ,
                Identity = user
            };

            User user1 = new User() { UserName = "Spec", Email = "Rek" };
            Teacher account1 = new Teacher()
            {
                FirstName = "Igorik"
                ,
                SecondName = "Igorenok"
                ,
                Age = 23
                ,
                Identity = user1
            };

            _users.Add(user);
            _users.Add(user1);

            _accounts.Add(account);
            _accounts.Add(account1);

            _teachers.Add(account);
            _teachers.Add(account1);

            _users.Add(
               new User
               {
                   UserName = "StudentUser",
               });

            _users.Add(
                new User
                {
                    UserName = "StudentUser1",
                });

            _users.Add(
                new User
                {
                    UserName = "TeacherUser",
                });

            _teachers.Add(
                new Teacher
                {
                    Identity = _users[2],
                    FirstName = "TeacherName1",
                    SecondName = "TeacherSecondName1",
                    Age = 31,
                    BirthDate = DateTime.Now
                });

            _facultatives.Add(
                new Facultative
                {
                    Lecturer = _teachers[0],
                    Description = "SomeDescription",
                    Duration = 60,
                    Status = FacultativeStatus.Registration,
                    Name = "FacultativeName1",
                    StartFacultative = DateTime.Now,
                    Theme = "FacultativeTheme",
                    Audience = new List<Student>()
                });

            _students.Add(new Student(new User() { UserName = "lol" }, "Stud33", "Stude32", 60, DateTime.Now));

            _reports.Add(new Report(_students[0], 60));
        }

        private void RestoreDate()
        {
            _users.Clear();
            _teachers.Clear();
            _accounts.Clear();
            _facultatives.Clear();
            _students.Clear();
            _reports.Clear();
            _reportBooks.Clear();
            InitData();
        }

        private void SetModelState(Controller controller, object ob)
        {
            var modelBinder = new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                     () => ob, ob.GetType()),
                ValueProvider = new NameValueCollectionValueProvider(
                       new NameValueCollection(), CultureInfo.InvariantCulture)
            };
            var binder = new DefaultModelBinder().BindModel(
                             new ControllerContext(), modelBinder);

            controller.ModelState.Clear();
            controller.ModelState.Merge(modelBinder.ModelState);
        }

        private Mock<ControllerContext> ControllerContext(string role
            , string HttpMethod
            , string identityName = "IvanNagibator007")
        {
            var requestMock = new Mock<HttpRequestBase>();

            requestMock.Setup(s => s.HttpMethod).Returns(HttpMethod);

            var userMock = new Mock<IPrincipal>();
            var IdentityMock = new Mock<IIdentity>();

            userMock.Setup(p => p.IsInRole(role)).Returns(true);
            userMock.Setup(p => p.IsInRole(It.IsNotIn(role))).Returns(false);

            IdentityMock.Setup(p => p.Name).Returns(identityName);

            userMock.Setup(p => p.Identity).Returns(IdentityMock.Object);

            MvcApplication mvcApplication = new MvcApplication();

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            contextMock.SetupGet(ctx => ctx.Request)
                       .Returns(requestMock.Object);

            var mockDispMod = new Mock<IDisplayMode>();
            var parArgMode = new Mock<ViewContext>();

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            return controllerContextMock;
        }

        [TestMethod]
        public void TestAccountRepository()
        {
            RestoreDate();

            var accountRepository = DependencyResolver.Current.GetService<IDefaultRepository<Account>>();

            Teacher account = new Teacher
            {
                FirstName = "Account1",
                SecondName = "Account1"
            };

            accountRepository.Add(account);

            Assert.IsNotNull(_accounts.Where(s => account.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(accountRepository.Get(account.Id).Id, account.Id);

            accountRepository.Remove(account);

            Assert.IsNull(accountRepository.Get(account.Id));
        }

        [TestMethod]
        public void TestTeacherRepository()
        {
            RestoreDate();

            var teacherRepository = DependencyResolver.Current.GetService<IDefaultRepository<Teacher>>();

            Teacher account = new Teacher
            {
                FirstName = "Account1",
                SecondName = "Account1"
            };

            teacherRepository.Add(account);

            Assert.IsNotNull(_teachers.Where(s => account.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(teacherRepository.Get(account.Id).Id, account.Id);

            teacherRepository.Remove(account);

            Assert.IsNull(teacherRepository.Get(account.Id));
        }

        [TestMethod]
        public void TestStudentRepository()
        {
            RestoreDate();

            var studentRepository = DependencyResolver.Current.GetService<IDefaultRepository<Student>>();

            Student account = new Student
            {
                FirstName = "Account1",
                SecondName = "Account1"
            };

            studentRepository.Add(account);

            Assert.IsNotNull(_students.Where(s => account.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(studentRepository.Get(account.Id).Id, account.Id);

            studentRepository.Remove(account);

            Assert.IsNull(studentRepository.Get(account.Id));
        }

        [TestMethod]
        public void TestReportRepository()
        {
            RestoreDate();

            var reportRepository = DependencyResolver.Current.GetService<IDefaultRepository<Report>>();

            Report report = new Report
            {
                Listener = _students[0],
                Mark = 60
            };

            reportRepository.Add(report);

            Assert.IsNotNull(_reports.Where(s => report.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(reportRepository.Get(report.Id).Id, report.Id);

            reportRepository.Remove(report);

            Assert.IsNull(reportRepository.Get(report.Id));
        }

        [TestMethod]
        public void TestReportBookRepository()
        {
            RestoreDate();

            var reportBookRepository = DependencyResolver.Current.GetService<IDefaultRepository<ReportBook>>();

            ReportBook reportBook = new ReportBook();

            reportBookRepository.Add(reportBook);

            Assert.IsNotNull(_reportBooks.Where(s => reportBook.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(reportBookRepository.Get(reportBook.Id).Id, reportBook.Id);

            reportBookRepository.Remove(reportBook);

            Assert.IsNull(reportBookRepository.Get(reportBook.Id));
        }

        [TestMethod]
        public void TestFacultativeRepository()
        {
            RestoreDate();

            var facultativeBookRepository = DependencyResolver.Current.GetService<IDefaultRepository<Facultative>>();

            Facultative facultative = new Facultative()
;
            facultativeBookRepository.Add(facultative);

            Assert.IsNotNull(_facultatives.Where(s => facultative.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(facultativeBookRepository.Get(facultative.Id).Id, facultative.Id);

            facultativeBookRepository.Remove(facultative);

            Assert.IsNull(facultativeBookRepository.Get(facultative.Id));
        }

        [TestMethod]
        public void TestAccountService()
        {
            RestoreDate();

            User student = new User() { UserName = "Student" };
            User teacher = new User() { UserName = "Teacher" };

            Account account = _accounts[0];
            Account account1 = _accounts[1];

            User user = _users[0];
            User user1 = _users[1];

            var accountService = DependencyResolver.Current.GetService<IAccountService>();

            Assert.AreEqual(accountService.GetAccount(_users[0].UserName).Id, account.Id);
            Assert.AreEqual(accountService.GetAccount(_users[1].UserName).Id, account1.Id);

            Assert.AreNotEqual(accountService.GetAccount(_users[1].UserName).Id, account.Id);
            Assert.AreNotEqual(accountService.GetAccount(_users[0].UserName).Id, account1.Id);

            accountService.CreateStudent(student, "lal", "bla", 60, DateTime.Now);
            accountService.CreateTeacher(teacher, "lala", "blaa", 60, DateTime.Now);

            Assert.IsNotNull(_students.Where(s => s.FirstName == "lal").FirstOrDefault());
            Assert.IsNotNull(_teachers.Where(s => s.FirstName == "lala").FirstOrDefault());

            Assert.IsNotNull(accountService.GetAccount("Student"));

            accountService.DeleteStudent(student);
            accountService.DeleteTeacher(teacher);

            Assert.IsNull(_students.Where(s => s.FirstName == "lal").FirstOrDefault());
            Assert.IsNull(_teachers.Where(s => s.FirstName == "lala").FirstOrDefault());
        }

        [TestMethod]
        public void TestStudentService()
        {
            RestoreDate();

            var studentService = DependencyResolver.Current.GetService<IStudentService>();

            Assert.AreEqual(studentService.GetAllFacultative().Count(), _facultatives.Count);

            Assert.AreEqual(studentService.GetFacultative(_facultatives[0]).Id, _facultatives[0].Id);

            Assert.AreEqual(_students[0].RegistrFacultatives.Count, 0);

            studentService.RegToFacultative(_students[0], _facultatives[0].Id);

            Assert.AreEqual(_students[0].RegistrFacultatives.Count, 1);
        }

        [TestMethod]
        public void TestTeacherService()
        {
            RestoreDate();

            var teachertService = DependencyResolver.Current.GetService<ITeacherService>();

            Teacher teacher = _teachers[0];
            User user = teacher.Identity;

            teachertService.AddFacultative(new Facultative
            {
                Lecturer = _teachers[0],
                Description = "SomeDescription",
                Duration = 70,
                Status = FacultativeStatus.Registration,
                Name = "FacultativeName2",
                StartFacultative = DateTime.Now,
                Theme = "FacultativeTheme",
                Audience = new List<Student>()
            }, teacher);

            Assert.AreNotEqual(teachertService.GetFacultative(_facultatives.Where(s => s.Name == "FacultativeName2").FirstOrDefault()), null);

            Teacher teacher1 = new Teacher();

            Assert.IsNotNull(_teachers.Where(s => s.FirstName == "asdsada"));

            Assert.IsNull(_facultatives[1].Log);

            teachertService.CreateReportBook("sdasdsad", _facultatives[1]);

            Assert.IsNotNull(_facultatives[1].Log);

            Assert.AreEqual(teachertService.GetFacultative(_facultatives[1]).Log.Name, "sdasdsad");

            int count = _facultatives.Count;

            teachertService.DeleteFacultative(_facultatives[0]);

            Assert.AreEqual(_facultatives.Count, count - 1);
        }

        [TestMethod]
        public void TestTeacherFacultativeController()
        {
            RestoreDate();

            TeacherFacultativeController teacherFacultativeController = new TeacherFacultativeController(DependencyResolver.Current.GetService<IAccountService>(), DependencyResolver.Current.GetService<ITeacherService>());

            teacherFacultativeController.ControllerContext = ControllerContext("Teacher", "GET").Object;

            var result = teacherFacultativeController.Index();

            Assert.AreEqual(teacherFacultativeController.HttpContext.User.IsInRole("Teacher"), true);
            Assert.AreNotEqual(teacherFacultativeController.HttpContext.User.IsInRole("Admin"), true);

            Assert.IsNotNull(((ViewResult)result).Model);
            Assert.AreEqual(((ViewResult)result).Model, _teachers.Where(s => s.Identity.UserName == "IvanNagibator007").FirstOrDefault());

        }

        [TestMethod]
        public void TestEditFacultativeController()
        {
            RestoreDate();

            EditFacultativeController editFacultativeController = new EditFacultativeController(DependencyResolver.Current.GetService<IAccountService>(), DependencyResolver.Current.GetService<ITeacherService>());

            editFacultativeController.ControllerContext = ControllerContext("Teacher", "POST").Object;

            Assert.AreEqual(editFacultativeController.HttpContext.User.IsInRole("Teacher"), true);
            Assert.AreNotEqual(editFacultativeController.HttpContext.User.IsInRole("Admin"), true);

            FacultativeViewModel facultativeVM = new FacultativeViewModel()
            {
                Name = "Blavla007",
                Description = "sadsdasdasdasdas",
                Theme = "asdsadasdasdasasd",
                Duration = 60,
                Status = FacultativeStatus.Registration,
                StartFacultative = DateTime.Now
            };

            SetModelState(editFacultativeController, facultativeVM);

            _teachers.Where(s => s.Identity.UserName == "IvanNagibator007").FirstOrDefault().MyFacultatives.Add(_facultatives[0]);

            Facultative facultative = _facultatives[0];

            var result = editFacultativeController.Index(facultativeVM, facultative.Id, "Change");

            Assert.AreEqual(facultative.Name, "Blavla007");
            Assert.AreEqual(facultative.Description, "sadsdasdasdasdas");
            Assert.AreEqual(facultative.Theme, "asdsadasdasdasasd");
            Assert.AreEqual(facultative.Duration, 60);
            Assert.AreEqual(facultative.Status, FacultativeStatus.Registration);

            facultativeVM = new FacultativeViewModel()
            {
                Name = "Bla",
                Description = "sfs",
                Theme = "4",
                Status = FacultativeStatus.Finished
            };

            SetModelState(editFacultativeController, facultativeVM);

            result = editFacultativeController.Index(facultativeVM, facultative.Id, "Change");

            Assert.AreEqual(facultative.Name, "Blavla007");
            Assert.AreEqual(facultative.Description, "sadsdasdasdasdas");
            Assert.AreEqual(facultative.Theme, "asdsadasdasdasasd");
            Assert.AreEqual(facultative.Status, FacultativeStatus.Finished);
        }

        [TestMethod]
        public void TestAddFacultativeController()
        {
            RestoreDate();

            AddFacultativeController addFacultativeController = new AddFacultativeController(
                  DependencyResolver.Current.GetService<IAccountService>()
                , DependencyResolver.Current.GetService<ITeacherService>());

            addFacultativeController.ControllerContext = ControllerContext("Teacher", "POST").Object;

            FacultativeViewModel facultativeVM = new FacultativeViewModel()
            {
                Name = "Bladasdsadasda",
                Description = "sfsasdsadasdasdas",
                Theme = "4asdasdsadasqewqeqweqweqweq",
                Status = FacultativeStatus.Registration,
                Duration = 60,
                StartFacultative = DateTime.Now
            };

            SetModelState(addFacultativeController, facultativeVM);

            RedirectToRouteResult result = addFacultativeController.Index(facultativeVM) as RedirectToRouteResult;

            Facultative facultative = _facultatives.Where(s => s.Name == "Bladasdsadasda").FirstOrDefault();

            Assert.AreEqual(facultative.Name, "Bladasdsadasda");
            Assert.AreEqual(facultative.Description, "sfsasdsadasdasdas");
            Assert.AreEqual(facultative.Theme, "4asdasdsadasqewqeqweqweqweq");
            Assert.AreEqual(facultative.Status, FacultativeStatus.Registration);
            Assert.AreEqual(facultative.Duration, 60);

            facultativeVM = new FacultativeViewModel()
            {
                Name = "a",
                Description = "sfsasdsadasdasdas",
                Theme = "4asdasdsadasqewqeqweqweqweq",
                Status = FacultativeStatus.Registration,
                Duration = 60,
                StartFacultative = DateTime.Now
            };

            SetModelState(addFacultativeController, facultativeVM);

            result = addFacultativeController.Index(facultativeVM) as RedirectToRouteResult;

            Assert.IsNull(_facultatives.Where(s => s.Name == "a").FirstOrDefault());


        }

        [TestMethod]
        public void TestAddLogController()
        {
            RestoreDate();

            AddLogController addLogController = new AddLogController(
                  DependencyResolver.Current.GetService<ITeacherService>()
                , DependencyResolver.Current.GetService<IAccountService>());

            addLogController.ControllerContext = ControllerContext("Teacher", "POST").Object;

            Facultative facultative = _facultatives.Where(s => s.Name == "FacultativeName1").FirstOrDefault();

            facultative.Audience.Add(_students[0]);

            ReportBookViewModel reportBookViewModel = new ReportBookViewModel
            {
                Elective = facultative,
                Reports = new List<ReportViewModel>
                {
                    new ReportViewModel
                    {
                        Id = _students[0].Id,
                        Mark = 60
                    }
                }

            };

            SetModelState(addLogController, reportBookViewModel);

            RedirectToRouteResult result = addLogController.Index(facultative.Id, reportBookViewModel) as RedirectToRouteResult;

            Assert.IsNotNull(facultative.Log);

            Assert.AreEqual(facultative.Log.Reports.Count, 1);

            Assert.AreEqual(facultative.Status, FacultativeStatus.Finished);

            Assert.AreEqual(facultative.Name, facultative.Log.Name);
        }

        [TestMethod]
        public void TestStudentFacultativeController()
        {
            RestoreDate();

            StudentFacultativeController controller = new StudentFacultativeController(
                  DependencyResolver.Current.GetService<IStudentService>());

            controller.ControllerContext = ControllerContext("Student", "POST", "lol").Object;

            var result = controller.Index(_facultatives[0].Id);

            Assert.IsNotNull(((ViewResult)result).Model);
        }

        [TestMethod]
        public void TestStudentFacultativesController()
        {
            RestoreDate();

            StudentFacultativesController controller = new StudentFacultativesController(
                  DependencyResolver.Current.GetService<IAccountService>(),
                  DependencyResolver.Current.GetService<IStudentService>());

            controller.ControllerContext = ControllerContext("Student", "POST", "lol").Object;

            var result = controller.Index();

            IEnumerable<Facultative> facultatives = (IEnumerable<Facultative>)(((ViewResult)result).Model);

            Assert.AreEqual(facultatives.Count(),0);
        }

        [TestMethod]
        public void TestStudentManagerController()
        {
            RestoreDate();

            StudentManagerController controller = new StudentManagerController(
                  DependencyResolver.Current.GetService<IAccountService>(),
                  DependencyResolver.Current.GetService<IStudentService>());

            controller.ControllerContext = ControllerContext("Student", "POST", "lol").Object;

            var result = controller.Index();

            IEnumerable<Facultative> facultatives = (IEnumerable<Facultative>)(((ViewResult)result).Model);

            Assert.AreEqual(_facultatives.Count, 1);

            Assert.AreEqual(_students[0].RegistrFacultatives.Count, 0);

            result = controller.RegisterFacultative(_facultatives[0].Id);

            Assert.AreEqual(_students[0].RegistrFacultatives.Count, 1);
        }

        [TestMethod]
        public void TestPersonalAreaController()
        {
            RestoreDate();

            PersonalAreaController controller = new PersonalAreaController(DependencyResolver.Current.GetService<IAccountService>());

            controller.ControllerContext = ControllerContext("Student", "POST", "lol").Object;

            var result = controller.Index();

            Account account = (Account)(((ViewResult)result).Model);

            Assert.AreEqual(account.FirstName, "Stud33");
            Assert.AreEqual(account.SecondName, "Stude32");
            Assert.AreEqual(account.Age, 60);

            PersonViewModel model = new PersonViewModel
            {
                FirstName = "4",
                SecondName = "1",
                Age = -5
            };

            SetModelState(controller, model);

            result = controller.Edit(model);

            Assert.AreEqual(account.FirstName, "Stud33");
            Assert.AreEqual(account.SecondName, "Stude32");
            Assert.AreEqual(account.Age, 60);

            model = new PersonViewModel
            {
                FirstName = "Stud333",
                SecondName = "Stud323",
                Age = 74
            };

            SetModelState(controller, model);

            result = controller.Edit(model);

            Assert.AreEqual(account.FirstName, "Stud333");
            Assert.AreEqual(account.SecondName, "Stud323");
            Assert.AreEqual(account.Age, 74);
        }
    }
}
