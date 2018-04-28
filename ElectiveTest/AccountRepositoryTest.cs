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
    public class AccountRepositoryTest
    {
        private List<User> _users = new List<User>();
        private List<Account> _accounts = new List<Account>();

        private Mock<ElectiveContext> mockContext;


        public AccountRepositoryTest()
        {
            mockContext = new Mock<ElectiveContext>();
            InitContext();
            InitData();
        }


        private void InitContext()
        {
            var mockSet_accounts = CreateMockSet(_accounts.AsQueryable());

            mockContext.Setup(c => c.Accounts).Returns(mockSet_accounts.Object);

            mockContext.Setup(c => c.Set<Account>()).Returns(mockContext.Object.Accounts);

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
        }

        private void RestoreDate()
        {
            _users.Clear();
            _accounts.Clear();
            InitData();
        }

        [Test]
        public void AddAccount()
        {
            RestoreDate();

            var accountRepository = new AccountRepository(mockContext.Object);

            Teacher account = new Teacher
            {
                FirstName = "Account1",
                SecondName = "Account1"
            };

            accountRepository.Add(account);

            Assert.IsNotNull(_accounts.Where(s => account.Id == s.Id).FirstOrDefault());
            Assert.AreEqual(accountRepository.Get(account.Id).Id, account.Id);
        }

        [Test]
        public void DeleteAccount()
        {
            RestoreDate();

            var accountRepository = new AccountRepository(mockContext.Object);

            Teacher account = new Teacher
            {
                FirstName = "Account1",
                SecondName = "Account1"
            };

            accountRepository.Add(account);

            accountRepository.Remove(account);

            Assert.IsNull(_accounts.Where(s => account.Id == s.Id).FirstOrDefault());
        }

        [Test]
        public void GetAccountById()
        {
            RestoreDate();

            var accountRepository = new AccountRepository(mockContext.Object);

            Teacher account = new Teacher
            {
                FirstName = "Account1",
                SecondName = "Account1"
            };

            accountRepository.Add(account);

            Assert.IsNotNull(_accounts.FirstOrDefault());
            Assert.IsNotNull(accountRepository.Get(account.Id));
        }
    }
}