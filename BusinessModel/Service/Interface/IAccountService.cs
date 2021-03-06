﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// set of methods fpr manages accounts
    /// </summary>
    public interface IAccountService
    {
        Account GetAccount(string identityName);

        void ChangeStateEntity<U>(U data) where U : class;

        void CreateStudent(User user, string firstName, string secondName, int age, DateTime birthDate);

        void DeleteStudent(User user);

        void DeleteTeacher(User user);

        void DeleteAdmin(User user);

        void CreateTeacher(User user, string firstName, string secondName, int age, DateTime birthDate);

        void SendMessage(string identityNameFrom, string identityNameTo, string theme, string message);

        Account GetAllMessages(string identityName);

        int GetNotReadMessages(string identityName);

        Message GetMessageReceive(string identityName, Guid id, ref bool notRead);

        Message GetMessageSend(string identityName, Guid id);
    }
}
