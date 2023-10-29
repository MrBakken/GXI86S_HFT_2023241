using GXI86S_HFT_2023241.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
                string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\veres\source\repos\GXI86S_HFT_2023241\GXI86S_HFT_2023241.Repository\bank.mdf;Integrated Security=True";


                builder.UseSqlServer(conn)
                .UseLazyLoadingProxies();
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(240);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(240);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(240);
                entity.Property(e => e.Email).HasMaxLength(240);
                entity.Property(e => e.Phone).HasMaxLength(240);
                entity.Property(e => e.BirthDate);
                entity.Property(e => e.Gender);

                entity.HasMany(c => c.Accounts) // Customer can have many accounts
                        .WithOne(a => a.Customer) // Account belongs to one customer
                        .HasForeignKey(a => a.CustomerId); // Foreign key in Account table
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(240);
                entity.Property(e => e.Balance);
                entity.Property(e => e.CreationDate);
                entity.Property(e => e.AccountType);

                entity.HasMany(c => c.Transactions) // Customer can have many accounts
                        .WithOne(a => a.Account) // Account belongs to one customer
                        .HasForeignKey(a => a.AccountId); // Foreign key in Account table
            });

            modelBuilder.Entity<Transaction>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.AccountId);
                    entity.Property(e => e.Date).IsRequired();
                    entity.Property(e => e.Amount).IsRequired();
                    entity.Property(e => e.Description).HasMaxLength(240);
                });

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "Teszt",
                    LastName = "Ügyfél",
                    Address = "Teszt cím",
                    Email = "teszt@valami.com",
                    Phone = "0620",
                    BirthDate = new DateTime(2002, 11, 10),
                    Gender = Genders.Female
                }
                    );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    CustomerId = 1, // Az ügyfél azonosítója
                    AccountNumber = "TESZT123",
                    Balance = 1000,
                    CreationDate = new DateTime(2023, 11, 12),
                    AccountType = AccountType.Current
                }
                        // Adjunk hozzá további teszt fiókokat itt
                        );

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = 1,
                    AccountId = 1, // Az Account azonosítója
                    Date = new DateTime(2023, 11, 12),
                    Amount = 1000,
                    Description = "teszt szöveg"
                }
                   // Adjunk hozzá további teszt fiókokat itt
                   );

            var jsonData = File.ReadAllText("customers.json");
            var customers = JsonSerializer.Deserialize<List<Customer>>(jsonData);

            modelBuilder.Entity<Customer>().HasData(customers.ToArray());

            //    modelBuilder.Entity<Customer>().HasData(new Customer[]
            //        {
            //new Customer("1$John$Doe$123 Main St$john@example.com$123-456-7890$1990-01-01$Male"),
            //new Customer("2$Jane$Smith$456 Elm St$jane@example.com$987-654-3210$1985-03-15$Female"),
            //new Customer("3$Michael$Johnson$789 Oak St$michael@example.com$555-555-5555$1982-07-20$Male"),
            //new Customer("4$Emily$Brown$234 Maple St$emily@example.com$444-333-2222$1995-05-10$Female"),
            //new Customer("5$William$Taylor$567 Birch St$william@example.com$777-888-9999$1988-11-30$Male"),
            //new Customer("6$Sarah$Johnson$567 Pine St$sarah@example.com$555-444-3333$1998-08-15$Female"),
            //new Customer("7$David$Williams$890 Cedar St$david@example.com$666-777-8888$1992-04-02$Male"),
            //new Customer("8$Olivia$Martinez$123 Spruce St$olivia@example.com$222-333-4444$1997-12-25$Female"),
            //new Customer("9$James$Smith$456 Oak St$james@example.com$123-123-1234$1993-06-08$Male"),
            //new Customer("10$Emma$Brown$789 Elm St$emma@example.com$777-888-9999$1996-02-17$Female"),
            //new Customer("11$William$Garcia$234 Maple St$william@example.com$111-222-3333$1991-10-11$Male"),
            //new Customer("12$Ava$Miller$456 Birch St$ava@example.com$444-555-6666$1994-07-03$Female"),
            //new Customer("13$Benjamin$Anderson$789 Redwood St$benjamin@example.com$333-444-5555$1999-03-27$Male"),
            //new Customer("14$Mia$Harris$123 Cedar St$mia@example.com$888-999-0000$1992-01-21$Female"),
            //new Customer("15$Elijah$Wilson$567 Oak St$elijah@example.com$777-777-7777$1987-09-14$Male"),
            //new Customer("16$Charlotte$Davis$890 Pine St$charlotte@example.com$555-666-7777$1995-11-29$Female"),
            //new Customer("17$Logan$Clark$234 Elm St$logan@example.com$999-888-7777$1993-08-04$Male"),
            //new Customer("18$Amelia$Johnson$456 Birch St$amelia@example.com$666-555-4444$1998-04-07$Female"),
            //new Customer("19$Lucas$Gonzalez$789 Spruce St$lucas@example.com$222-111-0000$1990-06-13$Male"),
            //new Customer("20$Sophia$Lopez$123 Redwood St$sophia@example.com$444-333-2222$1994-03-03$Female"),
            //            });

            //    modelBuilder.Entity<Account>().HasData(new Account[]
            //        {
            //        new Account()
            //            });
            //    modelBuilder.Entity<Transaction>().HasData(new Transaction[]
            //        {
            //        new Transaction()
            //            });
        }
    }
}

