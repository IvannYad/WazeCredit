using Microsoft.EntityFrameworkCore;
using WazeCredit.Models;

namespace WazeCredit.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<CreditApplication> CreditApplications { get; set; }
    }
}
