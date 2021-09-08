using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace API.Exttensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserByClaimsPrincipal(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return await input.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public static async Task<AppUser> FindUserByClaimsPrincipalIncludeAddress(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await input.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}