using Microsoft.EntityFrameworkCore;

namespace MyEverything.ThisMvc.Entities
{
    public class EverythingDbContext : DbContext
    {
        public EverythingDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ProjectInfo> ProjectsInfo { get; set; }
    }
}
