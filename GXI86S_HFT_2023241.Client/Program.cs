using GXI86S_HFT_2023241.Logic;
using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using System;

namespace GXI86S_HFT_2023241.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ctx = new BankDBContext();
            var repo = new CustomerRepository(ctx);
            var logic = new CustomerLogic(repo);

            Customer a = new Customer()
            {
                FirstName = "Dom",
                BirthDate = DateTime.Parse("2002.08.22")
            };
            logic.Create(a);
            logic.Delete(10);

            var items = logic.ReadAll();
            ;
        }
    }
}
