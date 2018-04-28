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
    public class FacultativeRepositoryTest
    {
        private List<Facultative> _Facultatives = new List<Facultative>();

        private Mock<ElectiveContext> mockContext;


        public FacultativeRepositoryTest()
        {
            mockContext = new Mock<ElectiveContext>();
            InitContext();
            InitData();
        }


        private void InitContext()
        {
            var mockSet_Facultatives = CreateMockSet(_Facultatives.AsQueryable());

            mockContext.Setup(c => c.Facultatives).Returns(mockSet_Facultatives.Object);

            mockContext.Setup(c => c.Set<Facultative>()).Returns(mockContext.Object.Facultatives);

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
                case Facultative acc:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _Facultatives.Add(de as Facultative));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _Facultatives.Remove(de as Facultative));
                    break;
            }

            return mockSet;
        }

        private void InitData()
        {
            
        }

        private void RestoreDate()
        {
            _Facultatives.Clear();
            InitData();
        }

        [Test]
        public void AddFacultative()
        {
            RestoreDate();

            var FacultativeRepository = new FacultativeRepository(mockContext.Object);

            Facultative Facultative = new Facultative();

            FacultativeRepository.Add(Facultative);

            Assert.IsNotNull(_Facultatives.Where(s => Facultative.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(FacultativeRepository.Get(Facultative.Id).Id, Facultative.Id);
        }

        [Test]
        public void DeleteFacultative()
        {
            RestoreDate();

            var FacultativeRepository = new FacultativeRepository(mockContext.Object);

            Facultative Facultative = new Facultative();

            FacultativeRepository.Add(Facultative);

            FacultativeRepository.Remove(Facultative);

            Assert.IsNull(_Facultatives.Where(s => Facultative.Id == s.Id).FirstOrDefault());
        }

        [Test]
        public void GetFacultativeById()
        {
            RestoreDate();

            var FacultativeRepository = new FacultativeRepository(mockContext.Object);

            Facultative Facultative = new Facultative();

            FacultativeRepository.Add(Facultative);

            Assert.IsNotNull(_Facultatives.FirstOrDefault());
            Assert.IsNotNull(FacultativeRepository.Get(Facultative.Id));
        }
    }
}