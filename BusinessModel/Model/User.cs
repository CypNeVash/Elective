using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessModel
{

    /// <summary>
    /// All roles for user
    /// </summary>
    public class Role
    {
        public const string Admin = "Admin";
        public const string Student = "Student";
        public const string Teacher = "Teacher";
    }


    ///<summary>
    ///Entity which contain personal 
    ///information of authorization
    ///</summary>
    public class User : IdentityUser
    {
        public Task<ClaimsIdentity> GenerateUserIdentity(UserManager<User> manager)
        {
            var userIdentity = manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

    }
}
