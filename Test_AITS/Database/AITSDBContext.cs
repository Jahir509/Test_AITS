using Microsoft.EntityFrameworkCore;
using System;
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
        public DbSet<SalesCommission> SalesCommissions { get; set; }
        public DbSet<AssociationCommission> AssociationCommissions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=DESKTOP-HG801OD;Database=AITS; Integrated Security=true; MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Walton"}
            );
            modelBuilder.Entity<Thana>().HasData(
                new Thana { Id = 1, Name = "Thana-A" }
            );
            modelBuilder.Entity<DealerType>().HasData(
                new DealerType { Id = 1, Name = "SIDC" },
                new DealerType { Id = 2, Name = "AMC" },
                new DealerType { Id = 3, Name = "BMC" }
            );
            modelBuilder.Entity<Commission>().HasData(
                new Commission { Id = 1, Type = "SIDC", FromAmount = 1000, ToAmount = 10000, Percentage = 6 },
                new Commission { Id = 2, Type = "SIDC", FromAmount = 10000, ToAmount = 30000, Percentage = (float)7.5 },
                new Commission { Id = 3, Type = "SIDC", FromAmount = 30000, ToAmount = 60000, Percentage = 9 },
                new Commission { Id = 4, Type = "SIDC", FromAmount = 60000, ToAmount = 999999, Percentage = 11 },
                new Commission { Id = 5, Type = "AMC", FromAmount = 5000, ToAmount = 50000, Percentage = 5 },
                new Commission { Id = 6, Type = "AMC", FromAmount = 50000, ToAmount = 999999, Percentage = 7 },
                new Commission { Id = 7, Type = "BMC", FromAmount = 5000, ToAmount = 999999, Percentage = 5 }
            );
            modelBuilder.Entity<Dealer>().HasData(
                new Dealer
                {
                    Id = 1,
                    Name = "Company",
                    CompanyId = 1,
                    DealerTypeId = 1,
                    ThanaId = 1,
                    IsSIDC = 1,
                    IsAMC = 0,
                    IsBMC = 0

                }
            );
            modelBuilder.Entity<DealerInfo>().HasData(
                new DealerInfo
                {
                    Id=1,
                    DealerCode="E-0001",
                    SellAmount =0,
                    DealerId =1,
                    AssociateOf ="Company",
                    SalesCommission =0,
                    InboundCommission =0,
                    OutboundCommission =0,
                    OrdinalCommission =0,
                    GBCommission =0,
                    Designation="Company"

                }
            );
            modelBuilder.Entity<SalesCommission>().HasData(
                new SalesCommission { Id = 1, Designation = "B", Percentage = (float)12.5 },
                new SalesCommission { Id = 2, Designation = "C", Percentage = 15 },
                new SalesCommission { Id = 3, Designation = "D", Percentage = (float)17.5 },
                new SalesCommission { Id = 4, Designation = "E", Percentage = 20 },
                new SalesCommission { Id = 5, Designation = "F", Percentage = 21 },
                new SalesCommission { Id = 6, Designation = "G", Percentage = 22 },
                new SalesCommission { Id = 7, Designation = "H", Percentage = 23 },
                new SalesCommission { Id = 8, Designation = "I", Percentage = 24 },
                new SalesCommission { Id = 9, Designation = "J", Percentage = 25 },
                new SalesCommission { Id = 10, Designation = "K", Percentage = 26 }
            );
        }
    }
}
