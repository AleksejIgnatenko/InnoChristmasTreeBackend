using InnoChristmasTree.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoChristmasTree
{
    public class InnoChristmasTreeDbContext : DbContext
    {
        public DbSet<CongratulationEntity> Congratulations { get; set; }

        public InnoChristmasTreeDbContext(DbContextOptions<InnoChristmasTreeDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
