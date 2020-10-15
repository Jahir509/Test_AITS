using Microsoft.EntityFrameworkCore;
using Test_AITS.Models;

namespace Test_AITS.Database
{
    public class AITSDBContext:DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Thana> Thanas { get; set; }
        public DbSet<DealerType> DealerTypes { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Commission> Commissions { get; set; }

        public DbSet<DealerInfo> DealerInfos { get; set; }
        public DbSet<Sale> Sales { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=DESKTOP-HG801OD;Database=AITS; Integrated Security=true";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
