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
    public class StudentRepositoryTest
    {
        private List<User> _users = new List<User>();
        private List<Student> _Students = new List<Student>();

        private Mock<ElectiveContext> mockContext;


        public StudentRepositoryTest()
        {
            mockContext = new Mock<ElectiveContext>();
            InitContext();
            InitData();
        }


        private void InitContext()
        {
            var mockSet_Students = CreateMockSet(_Students.AsQueryable());

            mockContext.Setup(c => c.Students).Returns(mockSet_Students.Object);

            mockContext.Setup(c => c.Set<Student>()).Returns(mockContext.Object.Students);

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

                case Student acc:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _Students.Add(de as Student));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _Students.Remove(de as Student));
                    break;
            }

            return mockSet;
        }

        private void InitData()
        {
            User user = new User() { UserName = "IvanNagibator007", Email = "IvanNagibator007" };
            Student Student = new Student()
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
            Student Student1 = new Student()
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

            _Students.Add(Student);
            _Students.Add(Student1);
        }

        private void RestoreDate()
        {
            _users.Clear();
            _Students.Clear();
            InitData();
        }

        [Test]
        public void AddStudent()
        {
            RestoreDate();

            var StudentRepository = new StudentRepository(mockContext.Object);

            Student Student = new Student
            {
                FirstName = "Student1",
                SecondName = "Student1"
            };

            StudentRepository.Add(Student);

            Assert.IsNotNull(_Students.Where(s => Student.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(StudentRepository.Get(Student.Id).Id, Student.Id);
        }

        [Test]
        public void DeleteStudent()
        {
            RestoreDate();

            var StudentRepository = new StudentRepository(mockContext.Object);

            Student Student = new Student
            {
                FirstName = "Student1",
                SecondName = "Student1"
            };

            StudentRepository.Add(Student);

            StudentRepository.Remove(Student);

            Assert.IsNull(_Students.Where(s => Student.Id == s.Id).FirstOrDefault());
        }

        [Test]
        public void GetStudentById()
        {
            RestoreDate();

            var StudentRepository = new StudentRepository(mockContext.Object);

            Student Student = new Student
            {
                FirstName = "Student1",
                SecondName = "Student1"
            };

            StudentRepository.Add(Student);

            Assert.IsNotNull(_Students.FirstOrDefault());
            Assert.IsNotNull(StudentRepository.Get(Student.Id));
        }
    }
}