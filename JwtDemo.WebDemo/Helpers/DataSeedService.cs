using System.Threading.Tasks;
using JwtDemo.WebDemo.EF.Models;
using Microsoft.AspNetCore.Identity;

namespace JwtDemo.WebDemo.Helpers
{
    public class DataSeedService : IDataSeedService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DataSeedService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
            var userBob = new ApplicationUser
            {
                Email = "bob@example.com",
                UserName = "bob@example.com"
            };

            var userAlice = new ApplicationUser
            {
                Email = "alice@example.com",
                UserName = "alice@example.com"
            };

            await _userManager.CreateAsync(userBob, "Password1!");
            await _userManager.CreateAsync(userAlice, "Password1!");

            await _roleManager.CreateAsync(new ApplicationRole { Name = "USER" });
            await _roleManager.CreateAsync(new ApplicationRole { Name = "ADMIN" });

            await _userManager.AddToRoleAsync(userBob, "USER");
            await _userManager.AddToRoleAsync(userAlice, "USER");
            await _userManager.AddToRoleAsync(userAlice, "ADMIN");
        }
    }
}
