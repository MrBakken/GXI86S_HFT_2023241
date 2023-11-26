using GXI86S_HFT_2023241.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GXI86S_HFT_2023241.Repository
{
    public class BankDBContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Transaction> Transactions { get; set; }


        public BankDBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseLazyLoadingProxies()
                    .UseInMemoryDatabase("bank");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(240);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(240);
                entity.Property(e => e.Email).HasMaxLength(240);
                entity.Property(e => e.Phone).HasMaxLength(240);
                entity.Property(e => e.BirthDate);
                entity.Property(e => e.Gender).HasConversion<string>();

                entity.HasMany(c => c.Accounts)
                        .WithOne(a => a.Customer)
                        .HasForeignKey(a => a.CustomerId);
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountNumber_ID);
                entity.Property(e => e.CurrencyType).IsRequired().HasMaxLength(240);
                entity.Property(e => e.Balance).HasConversion<string>();
                entity.Property(e => e.CreationDate);
                entity.Property(e => e.AccountType).HasConversion<string>();

                entity.HasMany(c => c.Transactions)
                        .WithOne(a => a.Account) 
                        .HasForeignKey(a => a.AccountId);

                entity.HasOne(t => t.Customer)
                    .WithMany(a => a.Accounts)
                    .HasForeignKey(t => t.CustomerId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountId);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(240);

                entity.HasOne(t => t.Account)
                    .WithMany(a => a.Transactions)
                    .HasForeignKey(t => t.AccountId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "Hanna",
                    LastName = "Woods",
                    Email = "Hanna@example.com",
                    Phone = "06-20-493-3449",
                    BirthDate = new DateTime(2002, 11, 10),
                    Gender = Genders.Female
                }
                    );

            modelBuilder.Entity<Customer>().HasData(new Customer[]
                {
            new ("2$John$Doe$john@example.com$123-456-7890$1990-01-01$Male"),

            new ("3$Michael$Johnson$michael@example.com$555-555-5555$1982-07-20$Male"),

            new ("4$Emily$Brown$emily@example.com$444-333-2222$1995-05-10$Female"),

            new ("5$Annie$Taylor$Annie@example.com$777-888-9999$1988-11-30$Female"),
                    });


            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountNumber_ID = 1325313,
                    CustomerId = 1,
                    CurrencyType = CurrencyEnum.EUR,
                    Balance = 1000,
                    CreationDate = new DateTime(2023, 11, 12),
                    AccountType = AccountTypeEnum.Savings
                }
                        );

            modelBuilder.Entity<Account>().HasData(new Account[]
                {
                    new ("1325345$1$HUF$62424$2011.04.02$Current"),
                    new ("0131314$1$EUR$4143431$2003.05.12$Savings"),
                    new ("0414424$2$HUF$21414$2014.05.19$Current"),
                    new ("9238446$2$EUR$14235$2023.05.18$Savings"),
                    new ("6352719$3$HUF$-43$2014.02.16$Current"),
                    new ("7458398$3$HUF$142545$2013.05.15$Savings"),
                    new ("3467425$4$EUR$2425636$2011.08.28$Current"),
                    new ("0941567$4$HUF$142356$2011.07.14$Savings"),
                    new ("1014251$4$HUF$14256346$2016.03.19$Savings"),
                    new ("1679531$5$EUR$13563$2011.07.15$Current"),
                    new ("1224551$5$HUF$746321$2012.02.20$Current"),
                    new ("1352566$5$HUF$246242$2017.05.1$Savings"),
                    });


            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = 1,
                    AccountId = 1352566,
                    Date = new DateTime(2023, 11, 12),
                    Amount = 1000,
                    Description = "Bevásárlás"
                }
                   );

            modelBuilder.Entity<Transaction>().HasData(new Transaction[]
                {
                    new ("02$1325313$2023.02.28 14:45:20$-123$Uzsonna"),
                    new ("03$1325313$2023.01.15 08:30:00$14254$Fizetés"),

                    new ("04$0131314$2023.03.10 18:00:45$15334$EURnics"),
                    new ("05$0131314$2023.04.05 09:15:30$-153$"),

                    new ("06$0414424$2023.05.20 12:00:00$145425$Felújítás"),
                    new ("07$0414424$2023.06.08 16:30:15$-1334254$"),

                    new ("08$9238446$2023.07.12 21:45:55$14234$Autó"),

                    new ("09$6352719$2023.08.29 10:10:05$-142354$"),

                    new ("10$7458398$2023.09.14 13:20:40$13414$Szerelő"),
                    new ("11$7458398$2023.10.22 17:00:25$154235$Családnak"),
                    new ("12$7458398$2023.11.09 08:55:50$-1143$Asztal"),

                    new ("13$3467425$2023.12.18 11:11:11$153365$Business"),

                    new ("14$0941567$2022.01.05 10:15:30$6235414$Tuti"),
                    new ("15$0941567$2022.02.18 14:30:45$-51556$Tippmix"),
                    new ("16$0941567$2022.03.20 18:45:15$72413$Szárazzsemle"),

                    new ("17$1014251$2022.04.08 09:00:30$16345$"),

                    new ("18$1679531$2022.05.15 12:30:00$-62626$valami"),
                    new ("19$1679531$2022.06.28 16:15:20$-142524$"),
                    new ("20$1679531$2023.08.29 10:10:05$1454$ruha"),

                    new Transaction("21$1224551$2022.07.10 21:00:55$-41425$haszontalanság"),
                    new Transaction("22$1224551$2023.05.20 12:00:00$14352$Roller"),

                    new Transaction("23$1352566$2022.03.02 13:23:23$-13342323$Űrhajó"),
                    });
        }
    }
}

