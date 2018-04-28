using System;
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
using NUnit.Framework;


namespace ElectiveTest
{
    [TestFixture]
    public class TeacherRepositoryTest
    {
        private List<User> _users = new List<User>();
        private List<Teacher> _Teachers = new List<Teacher>();

        private Mock<ElectiveContext> mockContext;


        public TeacherRepositoryTest()
        {
            mockContext = new Mock<ElectiveContext>();
            InitContext();
            InitData();
        }


        private void InitContext()
        {
            var mockSet_Teachers = CreateMockSet(_Teachers.AsQueryable());

            mockContext.Setup(c => c.Teachers).Returns(mockSet_Teachers.Object);

            mockContext.Setup(c => c.Set<Teacher>()).Returns(mockContext.Object.Teachers);

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
            mockSet.As<IEnumerable<T>>().Setup(m => m.GetEnumerator()).Returns(dataQ.GetEnumerator());

            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);

            switch (temp)
            {
                case User us:

                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _users.Add(de as User));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _users.Remove(de as User));
                    break;

                case Teacher acc:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _Teachers.Add(de as Teacher));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _Teachers.Remove(de as Teacher));
                    break;
            }

            return mockSet;
        }

        private void InitData()
        {
            User user = new User() { UserName = "IvanNagibator007", Email = "IvanNagibator007" };
            Teacher Teacher = new Teacher()
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
            Teacher Teacher1 = new Teacher()
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

            _Teachers.Add(Teacher);
            _Teachers.Add(Teacher1);
        }

        private void RestoreDate()
        {
            _users.Clear();
            _Teachers.Clear();
            InitData();
        }

        [Test]
        public void AddTeacher()
        {
            RestoreDate();

            var TeacherRepository = new TeacherRepository(mockContext.Object);

            Teacher Teacher = new Teacher
            {
                FirstName = "Teacher1",
                SecondName = "Teacher1"
            };

            TeacherRepository.Add(Teacher);

            Assert.IsNotNull(_Teachers.Where(s => Teacher.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(TeacherRepository.Get(Teacher.Id).Id, Teacher.Id);
        }

        [Test]
        public void DeleteTeacher()
        {
            RestoreDate();

            var TeacherRepository = new TeacherRepository(mockContext.Object);

            Teacher Teacher = new Teacher
            {
                FirstName = "Teacher1",
                SecondName = "Teacher1"
            };

            TeacherRepository.Add(Teacher);

            TeacherRepository.Remove(Teacher);

            Assert.IsNull(_Teachers.Where(s => Teacher.Id == s.Id).FirstOrDefault());
        }

        [Test]
        public void GetTeacherById()
        {
            RestoreDate();

            var TeacherRepository = new TeacherRepository(mockContext.Object);

            Teacher Teacher = new Teacher
            {
                FirstName = "Teacher1",
                SecondName = "Teacher1"
            };

            TeacherRepository.Add(Teacher);

            Assert.IsNotNull(_Teachers.FirstOrDefault());
            Assert.IsNotNull(TeacherRepository.Get(Teacher.Id));
        }
    }
}