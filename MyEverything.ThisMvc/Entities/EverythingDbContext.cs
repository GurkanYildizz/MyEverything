using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyEverything.ThisMvc.Entities
{
    public class EverythingDbContext : IdentityDbContext<AdminLoginInfo>
    {
        public EverythingDbContext(DbContextOptions options) : base(options)
        {
        }

        
        public DbSet<ProjectInfo> ProjectsInfo { get; set; }
     
    }
}
