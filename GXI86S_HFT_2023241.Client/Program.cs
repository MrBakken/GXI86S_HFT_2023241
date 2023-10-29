using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using System;
using System.IO;
using System.Linq;

namespace GXI86S_HFT_2023241.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("teszt1");

            IRepository<Account> repo = new AccountRepository(new BankDBContext());
            var item =repo.ReadAll().ToArray();
            ;
        }
    }
}
