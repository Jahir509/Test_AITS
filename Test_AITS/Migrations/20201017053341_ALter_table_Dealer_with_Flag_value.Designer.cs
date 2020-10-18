﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Test_AITS.Database;

namespace Test_AITS.Migrations
{
    [DbContext(typeof(AITSDBContext))]
    [Migration("20201017053341_ALter_table_Dealer_with_Flag_value")]
    partial class ALter_table_Dealer_with_Flag_value
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Test_AITS.Models.Commission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FromAmount")
                        .HasColumnType("int");

                    b.Property<float>("Percentage")
                        .HasColumnType("real");

                    b.Property<int>("ToAmount")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Commissions");
                });

            modelBuilder.Entity("Test_AITS.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Test_AITS.Models.Dealer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("DealerTypeId")
                        .HasColumnType("int");

                    b.Property<int>("IsAMC")
                        .HasColumnType("int");

                    b.Property<int>("IsBMC")
                        .HasColumnType("int");

                    b.Property<int>("IsSIDC")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThanaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("DealerTypeId");

                    b.HasIndex("ThanaId");

                    b.ToTable("Dealers");
                });

            modelBuilder.Entity("Test_AITS.Models.DealerInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DealerCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DealerId")
                        .HasColumnType("int");

                    b.Property<int>("InboundCommission")
                        .HasColumnType("int");

                    b.Property<int>("OrdinalCommission")
                        .HasColumnType("int");

                    b.Property<int>("OutboundCommission")
                        .HasColumnType("int");

                    b.Property<int>("SalesCommission")
                        .HasColumnType("int");

                    b.Property<int>("SellAmount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DealerId");

                    b.ToTable("DealerInfos");
                });

            modelBuilder.Entity("Test_AITS.Models.DealerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DealerTypes");
                });

            modelBuilder.Entity("Test_AITS.Models.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DealerCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InboundCommission")
                        .HasColumnType("int");

                    b.Property<int>("OrdinalCommission")
                        .HasColumnType("int");

                    b.Property<int>("OutboundCommission")
                        .HasColumnType("int");

                    b.Property<int>("SalesCommission")
                        .HasColumnType("int");

                    b.Property<int>("SellAmount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Test_AITS.Models.Thana", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Thanas");
                });

            modelBuilder.Entity("Test_AITS.Models.Dealer", b =>
                {
                    b.HasOne("Test_AITS.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Test_AITS.Models.DealerType", "DealerType")
                        .WithMany()
                        .HasForeignKey("DealerTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Test_AITS.Models.Thana", "Thana")
                        .WithMany()
                        .HasForeignKey("ThanaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Test_AITS.Models.DealerInfo", b =>
                {
                    b.HasOne("Test_AITS.Models.Dealer", "Dealer")
                        .WithMany()
                        .HasForeignKey("DealerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
