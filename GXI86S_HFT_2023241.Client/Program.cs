using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using GXI86S_HFT_2023241.Logic;
using System;
using System.IO;
using System.Linq;

namespace GXI86S_HFT_2023241.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ctx = new BankDBContext();
            var repo = new CustomerRepository(ctx);
            var logic = new BankLogic(repo);

            var items = logic.ReadAll();
            ;
        }
    }
}
