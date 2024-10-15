using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Identity;
using System.Security.Claims;

namespace Store.Apis.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> manager , ClaimsPrincipal User)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null) { return null; }


            //To Include The Address in the Return with me  in controller
            var user = await manager.Users.Include(U=>U.Address).FirstOrDefaultAsync(U=>U.Email == userEmail);

            if (user == null) { return null; }

            return user;

        }
    }
}
