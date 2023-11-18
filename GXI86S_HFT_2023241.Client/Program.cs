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
            var repoC = new CustomerRepository(ctx);
            var repoA = new AccountRepository(ctx);
            var repoT = new TransactionRepository(ctx);

            var logicC = new CustomerLogic(repoC);
            var logicA = new AccountLogic(repoA);
            var logicT = new TransactionLogic(repoT);

            Customer a = new Customer()
            {
                FirstName = "Dom",
                BirthDate = DateTime.Parse("2002.08.22")
            };
            logicC.Create(a);

            Account b = new Account()
            {
                Balance = 1000,
            };
            logicA.Create(b);

            var items = logicC.ReadAll();
            ;
        }
    }
}
