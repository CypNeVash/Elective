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
    public class ReportBookBookRepositoryTest
    {
        private List<ReportBook> _ReportBooks = new List<ReportBook>();

        private Mock<ElectiveContext> mockContext;


        public ReportBookBookRepositoryTest()
        {
            mockContext = new Mock<ElectiveContext>();
            InitContext();
            InitData();
        }


        private void InitContext()
        {
            var mockSet_ReportBooks = CreateMockSet(_ReportBooks.AsQueryable());

            mockContext.Setup(c => c.ReportBooks).Returns(mockSet_ReportBooks.Object);

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
            mockSet.As<IEnumerable<T>>().Setup(m => m.GetEnumerator()).Returns(dataQ.GetEnumerator());

            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);

            switch (temp)
            {
                case ReportBook acc:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _ReportBooks.Add(de as ReportBook));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _ReportBooks.Remove(de as ReportBook));
                    break;
            }

            return mockSet;
        }

        private void InitData()
        {
            
        }

        private void RestoreDate()
        {
            _ReportBooks.Clear();
            InitData();
        }

        [Test]
        public void AddReportBook()
        {
            RestoreDate();

            var ReportBookRepository = new ReportBookRepository(mockContext.Object);

            ReportBook reportBook = new ReportBook();

            ReportBookRepository.Add(reportBook);

            Assert.IsNotNull(_ReportBooks.Where(s => reportBook.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(ReportBookRepository.Get(reportBook.Id).Id, reportBook.Id);
        }

        [Test]
        public void DeleteReportBook()
        {
            RestoreDate();

            var ReportBookRepository = new ReportBookRepository(mockContext.Object);

            ReportBook reportBook = new ReportBook();

            ReportBookRepository.Add(reportBook);

            ReportBookRepository.Remove(reportBook);

            Assert.IsNull(_ReportBooks.Where(s => reportBook.Id == s.Id).FirstOrDefault());
        }

        [Test]
        public void GetReportBookById()
        {
            RestoreDate();

            var ReportBookRepository = new ReportBookRepository(mockContext.Object);

            ReportBook reportBook = new ReportBook();

            ReportBookRepository.Add(reportBook);

            Assert.IsNotNull(_ReportBooks.FirstOrDefault());
            Assert.IsNotNull(ReportBookRepository.Get(reportBook.Id));
        }
    }
}