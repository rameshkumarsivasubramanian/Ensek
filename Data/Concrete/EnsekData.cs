using Ensek.Models;
using Microsoft.EntityFrameworkCore;

namespace Ensek.Data.Concrete
{
    public class EnsekData : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeterReading>().HasKey(r => new { r.AccountId, r.MeterReadingDateTime });
        }

        public EnsekData(DbContextOptions<EnsekData> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
    }
}
