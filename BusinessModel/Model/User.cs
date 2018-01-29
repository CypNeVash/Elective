using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessModel
{
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
