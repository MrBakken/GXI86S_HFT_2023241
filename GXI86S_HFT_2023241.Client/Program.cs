using ConsoleTools;
using GXI86S_HFT_2023241.Logic;
using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GXI86S_HFT_2023241.Client
{
    internal class Program
    {
        static AccountLogic AccountLogic;
        static CustomerLogic CustomerLogic;
        static TransactionLogic TransactionLogic;

        static void Create(string entity)
        {
            Console.WriteLine(entity + " create");
            Console.ReadLine();
        }
        static void List(string entity)
        {
            switch (entity)
            {
                case "Customer":

                    var cusitems = CustomerLogic.ReadAll();
                    Console.WriteLine("{0,-10} |{1,-10} |{2,-24} |{3,-20} |{4,-16} |{5,-7}", "FirstName", "LastName", "Email", "Phone", "BirthDate", "Gender");
                    Console.WriteLine(new string('-', 100));
                    foreach (var item in cusitems)
                    {
                        Console.WriteLine("{0,-10} |{1,-10} |{2,-24} |{3,-20} |{4,-16} |{5,-7}", item.FirstName, item.LastName, item.Email, item.Phone, item.BirthDate.ToShortDateString(), item.Gender);
                    }
                    break;
                case "Account":

                    var Accitems = AccountLogic.ReadAll();
                    Console.WriteLine("{0,-13} |{1,-20} |{2,-13} |{3,-11}", "AccountNum", "Balance(Currency)", "AccountType", "CreationDate");
                    Console.WriteLine(new string('-', 100));
                    foreach (var item in Accitems)
                    {
                        string InCurrencyType = item.Balance.ToString() + " " + item.CurrencyType;
                        Console.WriteLine("{0,-13} |{1,-20} |{2,-13} |{3,-11}", item.AccountNumber_ID, InCurrencyType, item.AccountType, item.CreationDate.ToShortDateString());
                    }
                    break;
                case "Transaction":
                    var tranitems = TransactionLogic.ReadAll();
                    Console.WriteLine("{0,-13} |{1,-30} |{2,-13} |{3,-30}", "AccountId", "Date", "Amount", "Description");
                    Console.WriteLine(new string('-', 100));
                    foreach (var item in tranitems)
                    {
                        Console.WriteLine("{0,-13} |{1,-30} |{2,-13} |{3,-30}", item.AccountId, item.Date, item.Amount, item.Description);
                    }
                    break;
                default:
                    break;
            }
            Console.ReadLine();
        }
        static void Update(string entity)           //Nem müködik
        {
            switch (entity)
            {
                case "Customer":
                    Customer customer = new Customer();
                    CustomerLogic.Update(customer);
                    break;
                case "Account":
                    Account account = new Account();
                    AccountLogic.Update(account);
                    break;
                case "Transaction":
                    Transaction transaction = new Transaction();
                    TransactionLogic.Update(transaction);
                    break;
                default:
                    break;
            }
            Console.ReadLine();
        }
        static void Delete(string entity)
        {
            Console.WriteLine("Which ID item do you want to delete?\nÍrja ide: ");
            int IDforDelete = int.Parse(Console.ReadLine());
            switch (entity)
            {
                case "Customer":
                    CustomerLogic.Delete(IDforDelete);
                    break;
                case "Account":
                    AccountLogic.Delete(IDforDelete);
                    break;
                case "Transaction":
                    TransactionLogic.Delete(IDforDelete);
                    break;
                default:
                    break;
            }
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            var ctx = new BankDBContext();

            var CustumerRepo = new CustomerRepository(ctx);
            var AccRepo = new AccountRepository(ctx);
            var TranRepo = new TransactionRepository(ctx);

            CustomerLogic = new CustomerLogic(CustumerRepo);
            AccountLogic = new AccountLogic(AccRepo);
            TransactionLogic = new TransactionLogic(TranRepo, AccRepo);

            var CustomerSubMenu = new ConsoleMenu(args, level: 1)
                .Add("List", () => List("Customer"))
                .Add("Create", () => Create("Customer"))
                .Add("Delete", () => Delete("Customer"))
                .Add("Update", () => Update("Customer"))
                .Add("Exit", ConsoleMenu.Close);

            var AccountSubMenu = new ConsoleMenu(args, level: 1)
                .Add("List", () => List("Account"))
                .Add("Create", () => Create("Account"))
                .Add("Delete", () => Delete("Account"))
                .Add("Update", () => Update("Account"))
                .Add("Exit", ConsoleMenu.Close);

            var TransactionSubMenu = new ConsoleMenu(args, level: 1)
                .Add("List", () => List("Transaction"))
                .Add("Create", () => Create("Transaction"))
                .Add("Delete", () => Delete("Transaction"))
                .Add("Update", () => Update("Transaction"))
                .Add("Exit", ConsoleMenu.Close);


            var menu = new ConsoleMenu(args, level: 0)
                .Add("Customer", () => CustomerSubMenu.Show())
                .Add("Account", () => AccountSubMenu.Show())
                .Add("Transaction", () => TransactionSubMenu.Show())
                .Add("Exit", ConsoleMenu.Close);

            menu.Show();

        }
    }
}