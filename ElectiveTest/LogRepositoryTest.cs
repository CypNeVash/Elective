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
    public class LogRepositoryTest
    {
        private List<Log> _Logs = new List<Log>();

        private Mock<ElectiveContext> mockContext;


        public LogRepositoryTest()
        {
            mockContext = new Mock<ElectiveContext>();
            InitContext();
            InitData();
        }


        private void InitContext()
        {
            var mockSet_Logs = CreateMockSet(_Logs.AsQueryable());

            mockContext.Setup(c => c.Logs).Returns(mockSet_Logs.Object);

            mockContext.Setup(c => c.Set<Log>()).Returns(mockContext.Object.Logs);

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
                case Log acc:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _Logs.Add(de as Log));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _Logs.Remove(de as Log));
                    break;
            }

            return mockSet;
        }

        private void InitData()
        {
            
        }

        private void RestoreDate()
        {
            _Logs.Clear();
            InitData();
        }

        [Test]
        public void AddLog()
        {
            RestoreDate();

            var LogRepository = new LogRepository(mockContext.Object);

            Log Log = new Log();

            LogRepository.Add(Log);

            Assert.IsNotNull(_Logs.Where(s => Log.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(LogRepository.Get(Log.Id).Id, Log.Id);
        }

        [Test]
        public void DeleteLog()
        {
            RestoreDate();

            var LogRepository = new LogRepository(mockContext.Object);

            Log Log = new Log();

            LogRepository.Add(Log);

            LogRepository.Remove(Log);

            Assert.IsNull(_Logs.Where(s => Log.Id == s.Id).FirstOrDefault());
        }

        [Test]
        public void GetLogById()
        {
            RestoreDate();

            var LogRepository = new LogRepository(mockContext.Object);

            Log Log = new Log();

            LogRepository.Add(Log);

            Assert.IsNotNull(_Logs.FirstOrDefault());
            Assert.IsNotNull(LogRepository.Get(Log.Id));
        }
    }
}