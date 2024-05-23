using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data;

namespace Talabat.API.Extension
{
   
    public static class UserManagerExtension
    {
       

      
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager , ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u => u.Adress).SingleOrDefaultAsync(u => u.Email == email);
          
            return user;
        }

    }
}
