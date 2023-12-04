using ConsoleTools;
using GXI86S_HFT_2023241.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace GXI86S_HFT_2023241.Client
{
    internal class Program
    {
        static RestService rest;

        static void Main(string[] args)
        {
                rest = new RestService("http://localhost:34372/", "account");
            
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

        static void Create(string entity)       //befejezni
        {

            switch (entity)
            {
                case "Customer":
                    Console.Write("Enter a new Customer's Firstname.\nWrite here: ");
                    string NewFirstName = Console.ReadLine();
                    Console.Write("Enter a new Customer's Lastname.\nWrite here: ");
                    string NewLastName = Console.ReadLine();
                    Console.Write("Enter a new Customer's email address.\nWrite here: ");
                    string NewEmail = Console.ReadLine();
                    Console.Write("Enter a new Customer's Phone number.\nWrite here: ");
                    string NewPhone = Console.ReadLine();
                    Console.Write("Enter a new Customer's BirthDate.   Format(1900.01.01)\nWrite here: ");
                    bool correctformat = false;
                    DateTime NewBirthDate = DateTime.Parse("1900.01.01");
                    while (!correctformat)
                    {
                        try
                        {
                            string NewDateToTransfer = Console.ReadLine();
                            NewBirthDate = DateTime.Parse(NewDateToTransfer);
                            correctformat = true;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Nem jó formátum!");
                        }
                    }
                    Console.Write("Enter a new Customer's Gender.  Format:(Male/Female)\nWrite here: ");
                    correctformat = false;
                    Genders NewGender = Genders.Female;
                    while (!correctformat)
                    {
                        try
                        {
                            string NewGenderTrans = Console.ReadLine();
                            NewGender = (Genders)Enum.Parse(typeof(Genders), NewGenderTrans, true);

                            correctformat = true;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Nem jól írtad!");
                        }

                    }

                    rest.Post(new Customer() { FirstName = NewFirstName, LastName= NewLastName, Email =NewEmail, Phone = NewPhone, BirthDate = NewBirthDate, Gender =NewGender }, "customer");
                    break;

                case "Account":
                    Console.WriteLine("Enter a new CurrencyType(EUR\\HUF)\nWrite here: ");
                    CurrencyEnum NewAccCurrency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), Console.ReadLine());
                    rest.Post(new Account() { CreationDate = DateTime.Now, CurrencyType = NewAccCurrency }, "account");
                    break;

                case "Transaction":
                    Console.WriteLine("Enter an Amount\nWrite here: ");
                    int newAmount = int.Parse(Console.ReadLine());
                    rest.Post(new Transaction() { Amount = newAmount,Date = DateTime.Now }, "transaction");
                    break;

                default:
                    break;
            }
            Console.ReadLine();
        }
    
        static void List(string entity)         //befejezni
        {
            switch (entity)
            {
                case "Customer":
                    List<Customer> Customers = rest.Get<Customer>("customer");
                    Console.WriteLine("{0,-10} |{1,-10} |{2,-24} |{3,-20} |{4,-16} |{5,-7}", "FirstName", "LastName", "Email", "Phone", "BirthDate", "Gender");
                    Console.WriteLine(new string('-', 100));
                    foreach (var item in Customers)
                    {
                        Console.WriteLine("{0,-10} |{1,-10} |{2,-24} |{3,-20} |{4,-16} |{5,-7}", item.FirstName, item.LastName, item.Email, item.Phone, item.BirthDate.ToShortDateString(), item.Gender);
                    }
                    break;

                case "Account":
                    List<Account> Accounts = rest.Get<Account>("account");
                    Console.WriteLine("{0,-13} |{1,-20} |{2,-13} |{3,-11}", "AccountNum", "Balance(Currency)", "AccountType", "CreationDate");
                    Console.WriteLine(new string('-', 100));
                    foreach (var item in Accounts)
                    {
                        string InCurrencyType = item.Balance.ToString() + " " + item.CurrencyType;
                        Console.WriteLine("{0,-13} |{1,-20} |{2,-13} |{3,-11}", item.AccountNumber_ID, InCurrencyType, item.AccountType, item.CreationDate.ToShortDateString());
                    }
                    break;

                case "Transaction":
                    List<Transaction> Transactions = rest.Get<Transaction>("transaction");
                    Console.WriteLine("{0,-13} |{1,-30} |{2,-13} |{3,-30}", "AccountId", "Date", "Amount", "Description");
                    Console.WriteLine(new string('-', 100));
                    foreach (var item in Transactions)
                    {
                        Console.WriteLine("{0,-13} |{1,-30} |{2,-13} |{3,-30}", item.AccountId, item.Date, item.Amount, item.Description);
                    }
                    break;
                default:
                    break;
            }
            Console.ReadLine();
        }
        static void Update(string entity)               //befejezni
        {
            switch (entity)
            {
                case "Customer":
                    Console.Write("Enter Customer's id to update: ");
                    int id = int.Parse(Console.ReadLine());
                    Customer one = rest.Get<Customer>(id, "customer");
                    Console.Write($"New name [old: {one.LastName}]: ");
                    string name = Console.ReadLine();
                    one.LastName = name;
                    rest.Put(one, "actor");
                    break;

                case "Account":
                    Console.Write("Enter Account's id to update: ");
                    int accid = int.Parse(Console.ReadLine());
                    Account accone = rest.Get<Account>(accid, "account");
                    Console.Write($"New accountType [old: {accone.AccountType}] Write[Current/Savings]: ");
                    AccountTypeEnum acctype = (AccountTypeEnum)Enum.Parse(typeof(AccountTypeEnum), Console.ReadLine());
                    accone.AccountType = acctype;
                    rest.Put(accone, "actor");
                    break;

                case "Transaction":
                    Console.Write("Enter Transaction's id to update: ");
                    int tranid = int.Parse(Console.ReadLine());
                    Transaction tranone = rest.Get<Transaction>(tranid, "transaction");
                    Console.Write($"New description [old: {tranone.Description}]: ");
                    string newDescription = Console.ReadLine();
                    tranone.Description = newDescription;
                    rest.Put(tranone, "actor");
                    break;

                default:
                    break;
            }
            Console.ReadLine();
        }
        static void Delete(string entity)   //befejezni
        {
            Console.WriteLine($"Which {entity} ID item do you want to delete?\nWrite here: ");
            int id = int.Parse(Console.ReadLine());
            switch (entity)
            {
                case "Customer":
                    rest.Delete(id, "customer");
                    break;
                case "Account":
                    rest.Delete(id, "account");
                    break;
                case "Transaction":
                    rest.Delete(id, "transaction");
                    break;
                default:
                    break;
            }
            Console.ReadLine();
        }
    }
}