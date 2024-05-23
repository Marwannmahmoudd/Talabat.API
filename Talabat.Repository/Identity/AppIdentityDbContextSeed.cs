using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Marwan Mahmoud",
                    Email = "marwanMahmoud@gmail.com",
                    UserName = "Marwan.Mahmoud",
                    PhoneNumber = "01033552211"
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
            
        }
    }
}
