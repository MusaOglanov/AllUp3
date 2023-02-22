using Microsoft.EntityFrameworkCore;

namespace AllUp3.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }


    }
}
