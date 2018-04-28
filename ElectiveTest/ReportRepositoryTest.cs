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
    public class ReportRepositoryTest
    {
        private List<Report> _Reports = new List<Report>();

        private Mock<ElectiveContext> mockContext;


        public ReportRepositoryTest()
        {
            mockContext = new Mock<ElectiveContext>();
            InitContext();
            InitData();
        }


        private void InitContext()
        {
            var mockSet_Reports = CreateMockSet(_Reports.AsQueryable());

            mockContext.Setup(c => c.Reports).Returns(mockSet_Reports.Object);

            mockContext.Setup(c => c.Set<Report>()).Returns(mockContext.Object.Reports);

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
                case Report acc:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _Reports.Add(de as Report));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _Reports.Remove(de as Report));
                    break;
            }

            return mockSet;
        }

        private void InitData()
        {
            
        }

        private void RestoreDate()
        {
            _Reports.Clear();
            InitData();
        }

        [Test]
        public void AddReport()
        {
            RestoreDate();

            var reportRepository = new ReportRepository(mockContext.Object);

            Report report = new Report
            {
                Listener = new Student(),
                Mark = 60
            };

            reportRepository.Add(report);

            Assert.IsNotNull(_Reports.Where(s => report.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(reportRepository.Get(report.Id).Id, report.Id);
        }

        [Test]
        public void DeleteReport()
        {
            RestoreDate();

            var ReportRepository = new ReportRepository(mockContext.Object);

            Report report = new Report
            {
                Listener = new Student(),
                Mark = 60
            };


            ReportRepository.Add(report);

            ReportRepository.Remove(report);

            Assert.IsNull(_Reports.Where(s => report.Id == s.Id).FirstOrDefault());
        }

        [Test]
        public void GetReportById()
        {
            RestoreDate();

            var ReportRepository = new ReportRepository(mockContext.Object);

            Report report = new Report
            {
                Listener = new Student(),
                Mark = 60
            };

            ReportRepository.Add(report);

            Assert.IsNotNull(_Reports.FirstOrDefault());
            Assert.IsNotNull(ReportRepository.Get(report.Id));
        }
    }
}