using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BusinessModel
{
    /// <summary>
    ///     Drop and create database if model changes and
    ///     seed the database.
    ///</summary>
    class ElectiveContextInitializer : DropCreateDatabaseAlways<ElectiveContext>
    {
        protected override void Seed(ElectiveContext _context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            var UserManager = new UserManager<User>(new UserStore<User>(_context));

            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("Student"));
            roleManager.Create(new IdentityRole("Teacher"));


            var user = new User();
            user.UserName = "admin@nure.ua";
            user.Email = "admin@nure.ua";
            user.LockoutEnabled = true;

            _context.Accounts.Add(new Account(user, "sdas", "sadsad", 60, DateTime.Now));

            string userPWD = "Admin29++";

            UserManager.Create(user, userPWD);

            UserManager.AddToRole(user.Id, "Admin");

            user = new User();
            user.UserName = "parviz2@nure.ua";
            user.Email = "parviz2@nure.ua";
            user.LockoutEnabled = true;

            userPWD = "Parviz29++";

            UserManager.Create(user, userPWD);

            UserManager.AddToRole(user.Id, "Student");

            _context.Students.Add(new Student(user,"aaaaaa","aaaaa", 60, DateTime.Now));

            user = new User();
            user.UserName = "parviz@nure.ua";
            user.Email = "parviz@nure.ua";
            user.LockoutEnabled = true;

            userPWD = "Parviz29++";

            UserManager.Create(user, userPWD);

            UserManager.AddToRole(user.Id, "Teacher");

            

            _context.Facultatives.Add(new Facultative("Syp", "Nep","lalaala", FacultativeStatus.Registration, new Teacher(user, "bbbbbbb", "bbbbbbbb", 70, DateTime.Now), System.DateTime.Now,40));

            _context.SaveChanges();
        }
    }
}
