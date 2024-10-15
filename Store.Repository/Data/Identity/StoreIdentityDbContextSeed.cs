using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Data.Identity
{
    public static class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> userManager)
        {

            if (userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "gemiahmed@gmail.com",
                    DisplayName = "Ahmed Qassem",
                    UserName = "Ahmedg",
                    PhoneNumber = "01148341879",
                    Address = new Address()
                    {
                        FirstName = "Ahmed",
                        LastName = "Qassem",
                        City = "Maadi",
                        Country = "Egypt",
                        Street = "291",
                    }

                };
                // use The Requirement of the Identity user Password
                await userManager.CreateAsync(user, "P@$$w0rd123");

            }

         

        }

    }
}

