using Microsoft.EntityFrameworkCore;
using NST.Model;

namespace NST.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    { 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<PersonModel> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }
    }
}