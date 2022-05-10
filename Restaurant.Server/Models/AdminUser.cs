using Microsoft.AspNetCore.Identity;

namespace Restaurant.Server.Models
{
    /// <summary>
    /// Class for a staff user
    /// </summary>
    public class AdminUser : IdentityUser<int>
    {
        public AdminUser()
        {

        }
    }
}
