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
    public class MessageRepositoryTest
    {
        private List<Message> _Messages = new List<Message>();

        private Mock<ElectiveContext> mockContext;


        public MessageRepositoryTest()
        {
            mockContext = new Mock<ElectiveContext>();
            InitContext();
            InitData();
        }


        private void InitContext()
        {
            var mockSet_Messages = CreateMockSet(_Messages.AsQueryable());

            mockContext.Setup(c => c.Messages).Returns(mockSet_Messages.Object);

            mockContext.Setup(c => c.Set<Message>()).Returns(mockContext.Object.Messages);

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
                case Message acc:
                    mockSet.Setup(x => x.Add(It.IsAny<T>()))
                    .Callback((T de) => _Messages.Add(de as Message));

                    mockSet.Setup(x => x.Remove(It.IsAny<T>()))
                    .Callback((T de) => _Messages.Remove(de as Message));
                    break;
            }

            return mockSet;
        }

        private void InitData()
        {
            
        }

        private void RestoreDate()
        {
            _Messages.Clear();
            InitData();
        }

        [Test]
        public void AddMessage()
        {
            RestoreDate();

            var MessageRepository = new MessageRepository(mockContext.Object);

            Message Message = new Message();

            MessageRepository.Add(Message);

            Assert.IsNotNull(_Messages.Where(s => Message.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(MessageRepository.Get(Message.Id).Id, Message.Id);
        }

        [Test]
        public void DeleteMessage()
        {
            RestoreDate();

            var MessageRepository = new MessageRepository(mockContext.Object);

            Message Message = new Message();

            MessageRepository.Add(Message);

            MessageRepository.Remove(Message);

            Assert.IsNull(_Messages.Where(s => Message.Id == s.Id).FirstOrDefault());
        }

        [Test]
        public void GetMessageById()
        {
            RestoreDate();

            var MessageRepository = new MessageRepository(mockContext.Object);

            Message Message = new Message();

            MessageRepository.Add(Message);

            Assert.IsNotNull(_Messages.FirstOrDefault());
            Assert.IsNotNull(MessageRepository.Get(Message.Id));
        }
    }
}