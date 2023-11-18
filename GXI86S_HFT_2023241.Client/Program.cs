using GXI86S_HFT_2023241.Logic;
using GXI86S_HFT_2023241.Logic.InterfaceLogic;
using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using static GXI86S_HFT_2023241.Logic.CustomerLogic;

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
                Customer = a
            };
            logicA.Create(b);

            Transaction c = new Transaction()
            {
                Amount = -300,
                Account = b,
            };
            logicT.Create(c);


            var BirthdayInYear =logicC.GetCustomersWithBirthdayInYear(2002);

            foreach (var customer in BirthdayInYear)
            {
                Console.WriteLine($"Ügyfél neve: {customer.FirstName} {customer.LastName}, Születésnap: {customer.BirthDate}");
            }

            Console.WriteLine("\n");

            var customerTransactionInfo = logicC.GetCustomerTransactionInfo();

            foreach (var info in customerTransactionInfo)
            {
                Console.WriteLine($"Ügyfél ID: {info.CustomerId}, Név: {info.FirstName} {info.LastName}, Tranzakciók száma: {info.NumberOfTransactions}");
            }

            Console.WriteLine("\n");

            var customerAccountInfo = logicC.GetCustomersWithAccountsAndTransactions();

            foreach (var info in customerAccountInfo)
            {
                Console.WriteLine($"Ügyfél ID: {info.CustomerId}, Név: {info.FirstName} {info.LastName}");

                foreach (var account in info.Accounts)
                {
                    Console.WriteLine($"   Számla: {account.AccountNumber}, Tranzakciók száma: {account.TransactionCount}");
                }
            }
            Console.WriteLine("\n");

            var customerTransactionDetails = logicC.GetCustomerTransactionDetails();

            Console.WriteLine("{0,-20} {1,-20} {2,-20}", "Customer Name", "Total Amount", "Account Type");
            Console.WriteLine(new string('-', 60));

            foreach (var detail in customerTransactionDetails)
            {
                Console.WriteLine("{0,-20} {1,-20:C} {2,-20}", detail.CustomerName, detail.TotalTransactionAmount, detail.AccountType);
            }

            var items = logicC.ReadAll();
            ;
        }
    }
}
