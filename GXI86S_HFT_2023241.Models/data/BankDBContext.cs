using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GXI86S_HFT_2023241.Models.Data
{
    internal class BankDBContext : DbContext
    {
        public DbSet<Account> accounts { get; set; }
        public DbSet<Customer> customers { get; set; }

        public DbSet<Transaction> transactions { get; set; }

        //databasek pipa most kell inmemory vagy localdb, data mappa?, rétegzés:06video
    }
}
