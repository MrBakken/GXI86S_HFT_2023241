using ConsoleTools;
using GXI86S_HFT_2023241.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

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

            var NonCrudSubMenu = new ConsoleMenu(args, level: 1)
                .Add("Get Customers with Birthday in Year", () => GetCustomersWithBirthdayInYear())
                .Add("Get Customer Transaction Info", () => GetCustomerTransactionInfo())
                .Add("Get Customers with Accounts and Transactions", () => GetCustomersWithAccountsAndTransactions())
                .Add("Get Customer Transaction Details", () => GetCustomerTransactionDetails())
                .Add("Get Total Spending Last 30 Days", () => GetTotalSpendingLast30Days())
                .Add("Get Last Income Per Customer", () => GetLastIncomePerCustomer())
                .Add("Exit", ConsoleMenu.Close);


            var menu = new ConsoleMenu(args, level: 0)
                .Add("Customer", () => CustomerSubMenu.Show())
                .Add("Account", () => AccountSubMenu.Show())
                .Add("Transaction", () => TransactionSubMenu.Show())
                .Add("NonCrud", () => NonCrudSubMenu.Show())
                .Add("Exit", ConsoleMenu.Close);

            menu.Show();

        }

        static void Create(string entity)
        {
            try
            {
                switch (entity)
                {
                    case "Customer":
                        Console.Write("Enter a new Customer's Firstname.\nWrite here: ");
                        string NewFirstName = Console.ReadLine();

                        Console.Write("Enter a new Customer's Lastname.\nWrite here: ");
                        string NewLastName = Console.ReadLine();

                        Console.Write("Enter a new Customer's email address. If you want null wtrie: -\nWrite here: ");
                        string NewEmail = Console.ReadLine();
                        if (NewEmail == "-")
                        {
                            NewEmail = null;
                        }
                        Console.Write("Enter a new Customer's Phone number. If you want null wtrie: -\nWrite here: ");
                        string NewPhone = Console.ReadLine();
                        if (NewPhone == "-")
                        {
                            NewPhone = null;
                        }
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
                            catch (FormatException)
                            {
                                Console.WriteLine("Invalid date format. Please enter a valid date in the correct format.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                        }
                        Console.Write("Enter a new Customer's Gender.  Format:(Male/Female)\nWrite here: ");
                        correctformat = false;
                        Genders NewGender = Genders.Female;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewGenderIn = Console.ReadLine();
                                NewGender = (Genders)Enum.Parse(typeof(Genders), NewGenderIn, true);

                                correctformat = true;
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Invalid gender. Ex:{ex.Message}");
                            }
                        }
                        try
                        {

                            rest.Post(new Customer() { FirstName = NewFirstName, LastName = NewLastName, Email = NewEmail, Phone = NewPhone, BirthDate = NewBirthDate, Gender = NewGender }, "customer");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"The save failed. Cause:  {ex.Message}");
                        }
                        break;

                    case "Account":
                        #region Owner
                        Console.WriteLine("Enter a new Account's Owner(custumerid).\nWrite here: ");
                        correctformat = false;
                        int NewCustomerId = 0;
                        while (!correctformat)
                        {
                            try
                            {
                                NewCustomerId = int.Parse(Console.ReadLine());
                                try
                                {
                                    var tmp = rest.Get<Customer>(NewCustomerId, "customer");
                                }
                                catch (Exception)
                                {
                                    throw new Exception("This id not Exist!");
                                }
                                correctformat = true;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Invalid format. Please enter a valid integer.");
                            }
                            catch (OverflowException)
                            {
                                Console.WriteLine("Number is too large or too small for an integer.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        #endregion

                        #region CurrencyType
                        Console.WriteLine("Enter a new Account's CurrencyType(EUR\\HUF)\nWrite here: ");
                        correctformat = false;
                        CurrencyEnum NewAccCurrency = CurrencyEnum.HUF;
                        while (!correctformat)
                        {
                            try
                            {
                                string Input = Console.ReadLine().ToUpper();
                                NewAccCurrency = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), Input, true);
                                correctformat = true;
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine("Wrong input! Try again!");
                            }
                        }
                        #endregion

                        #region AccountType
                        Console.WriteLine("Enter a new Account's AccountType(Current\\Savings)\nWrite here: ");
                        correctformat = false;
                        AccountTypeEnum NewAccountType = AccountTypeEnum.Current;
                        while (!correctformat)
                        {
                            try
                            {
                                string Input = Console.ReadLine();
                                NewAccountType = (AccountTypeEnum)Enum.Parse(typeof(AccountTypeEnum), Input, true);
                                correctformat = true;
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine("Wrong input! Try again!");
                            }
                        }
                        #endregion
                        try
                        {
                            rest.Post(new Account() { CustomerId = NewCustomerId, CurrencyType = NewAccCurrency, AccountType = NewAccountType }, "account");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }

                        break;

                    case "Transaction":
                        #region Account's
                        Console.WriteLine("Enter a witch Account's transaction is it.\nWrite here: ");
                        correctformat = false;
                        int NewAccountId = 0;
                        while (!correctformat)
                        {
                            try
                            {
                                NewAccountId = int.Parse(Console.ReadLine());
                                try
                                {
                                    var tmp = rest.Get<Account>(NewAccountId, "account");
                                }
                                catch (Exception)
                                {
                                    throw new Exception("This id not Exist!");
                                }
                                correctformat = true;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Invalid format. Please enter a valid integer.");
                            }
                            catch (OverflowException)
                            {
                                Console.WriteLine("Number is too large or too small for an integer.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        #endregion

                        #region Amount
                        Console.WriteLine("Enter a new Transaction's Amount\nWrite here: ");
                        correctformat = false;
                        int NewAmount = 0;
                        while (!correctformat)
                        {
                            try
                            {
                                NewAmount = int.Parse(Console.ReadLine());
                                correctformat = true;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Invalid format. Please enter a valid integer.");
                            }
                            catch (OverflowException)
                            {
                                Console.WriteLine("Number is too large or too small for an integer.");
                            }
                        }
                        #endregion

                        #region Description
                        Console.WriteLine("Enter a new Transaction's Description. If you want null wtrie: -\nWrite here: ");
                        string NewDescription = Console.ReadLine();
                        if (NewDescription == "-")
                        {
                            NewDescription = null;
                        }
                        #endregion

                        try
                        {
                            rest.Post(new Transaction() { AccountId = NewAccountId, Amount = NewAmount, Description = NewDescription }, "transaction");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadLine();
        }
        static void List(string entity)
        {
            try
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
                        Console.WriteLine("{0,-11} | {1,-13} |{2,-20} |{3,-13} |{4,-11}", "CustomerId", "AccountNum", "Balance(Currency)", "AccountType", "CreationDate");
                        Console.WriteLine(new string('-', 100));
                        foreach (var item in Accounts)
                        {
                            string InCurrencyType = item.Balance.ToString() + " " + item.CurrencyType;
                            Console.WriteLine("{0,-11} |{1,-13} |{2,-20} |{3,-13} |{4,-11}", item.CustomerId, item.AccountNumber_ID, InCurrencyType, item.AccountType, item.CreationDate.ToShortDateString());
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadLine();
        }
        static void Update(string entity)
        {
            try
            {
                switch (entity)
                {
                    case "Customer":
                        #region Selecter
                        Console.Write("Enter Customer's id to update: ");
                        bool correctformat = false;
                        Customer one = new Customer();
                        int id = 0;
                        while (!correctformat)
                        {
                            try
                            {
                                id = int.Parse(Console.ReadLine());
                                one = rest.Get<Customer>(id, "customer");
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid format for CustomerId. Please enter a valid integer.");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Value for CustomerId is too large or too small for an integer.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Unexpected argument exception during API call.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Unexpected error: {ex.Message}");
                            }

                        }
                        #endregion

                        #region one.Id
                        Console.Write($"Enter the new Amount.[old: {one.Id}] Write - if you dont want to modify.\nWrite here: ");

                        while (!correctformat)
                        {
                            try
                            {
                                string NewID = Console.ReadLine();
                                if (NewID != "-")
                                {
                                    rest.Get<Customer>(int.Parse(NewID), "customer");
                                    one.Id = int.Parse(NewID);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine("The Input wrong: " + ex.Message + "Try Again!");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine("The Input wrong: " + ex.Message + "Try Again!");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message} Try another:");
                            }
                        }

                        #endregion

                        #region one.FirstName
                        Console.Write($"Enter the new Firstname.[old: {one.FirstName}] Write - if you dont want to modify.\nWrite here: ");
                        string NewFirstName = Console.ReadLine();
                        if (NewFirstName != "-")
                        {
                            one.FirstName = NewFirstName;
                        }
                        #endregion

                        #region one.LastName
                        Console.Write($"Enter the new LastName.[old: {one.LastName}] Write - if you dont want to modify.\nWrite here: ");
                        string NewLastName = Console.ReadLine();
                        if (NewLastName != "-")
                        {
                            one.LastName = NewLastName;
                        }
                        #endregion

                        #region one.Email
                        Console.Write($"Enter the new Email address.[old: {one.Email}] Write - if you dont want to modify.\nWrite here: ");
                        string NewEmail = Console.ReadLine();
                        if (NewEmail != "-")
                        {
                            one.Email = NewEmail;
                        }
                        #endregion

                        #region one.Phone
                        Console.Write($"Enter the new Phone number.[old: {one.Phone}] Write - if you dont want to modify.\nWrite here: ");
                        string NewPhone = Console.ReadLine();
                        if (NewPhone != "-")
                        {
                            one.Phone = NewPhone;
                        }
                        #endregion

                        #region one.BirthDate
                        Console.Write($"Enter the new BirthDate.[old: {one.BirthDate}] Write - if you dont want to modify. Format(1900.01.01)\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewDateToTransfer = Console.ReadLine();
                                if (NewDateToTransfer != "-")
                                {
                                    one.BirthDate = DateTime.Parse(NewDateToTransfer);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid date format. Please enter a valid date.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Unexpected error: {ex.Message}");
                            }

                        }
                        #endregion

                        #region one.Gender
                        Console.Write($"Enter the new Gender.[old: {one.Gender}] Write - if you dont want to modify. Format(Male/Female)\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewGenderIn = Console.ReadLine();
                                if (NewGenderIn != "-")
                                {
                                    one.Gender = (Genders)Enum.Parse(typeof(Genders), NewGenderIn, true);
                                }
                                correctformat = true;
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid value for Gender. Please enter a valid value from the Genders enum.");
                            }
                        }
                        #endregion

                        rest.Put(one, "customer");
                        break;

                    case "Account":

                        #region Selecter
                        Console.Write("Enter Account's id to update: ");
                        correctformat = false;
                        int accid = 0;
                        Account accone = new Account();
                        while (!correctformat)
                        {
                            try
                            {
                                accid = int.Parse(Console.ReadLine());
                                accone = rest.Get<Account>(accid, "account");
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid format for AccountId. Please enter a valid integer.");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Value for AccountId is too large or too small for an integer.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Unexpected argument exception during API call.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Unexpected error: {ex.Message}");
                            }
                        }
                        #endregion



                        #region accone.CustomerId
                        Console.Write($"Enter the new Owner(custumerid).[old: {accone.CustomerId}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewCustomerId = Console.ReadLine();
                                if (NewCustomerId != "-")
                                {
                                    accone.CustomerId = int.Parse(NewCustomerId);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid format for CustomerId. Please enter a valid integer.");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Value for CustomerId is too large or too small for an integer.");
                            }
                        }
                        #endregion

                        #region accone.AccountNumber_ID
                        Console.Write($"Enter the new AccountNumber_ID.[old: {accone.AccountNumber_ID}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewAccountNumber_ID = Console.ReadLine();
                                if (NewAccountNumber_ID != "-")
                                {
                                    rest.Get<Account>(int.Parse(NewAccountNumber_ID), "account");
                                    accone.AccountNumber_ID = int.Parse(NewAccountNumber_ID);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid format for AccountNumber_ID. Please enter a valid integer.");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Value for AccountNumber_ID is too large or too small for an integer.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message} Try another:");
                            }

                        }
                        #endregion

                        #region accone.CurrencyType
                        Console.Write($"Enter the new CurrencyType(EUR\\HUF).[old: {accone.CurrencyType}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewAccCurrency = Console.ReadLine();
                                if (NewAccCurrency != "-")
                                {
                                    accone.CurrencyType = (CurrencyEnum)Enum.Parse(typeof(CurrencyEnum), NewAccCurrency, true);
                                }
                                correctformat = true;
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid value for CurrencyEnum.");
                            }
                        }
                        #endregion

                        #region accone.AccountType
                        Console.Write($"Enter the new AccountType(Current\\Savings).[old: {accone.AccountType}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;

                        while (!correctformat)
                        {
                            try
                            {
                                string NewAccountType = Console.ReadLine();
                                if (NewAccountType != "-")
                                {
                                    accone.AccountType = (AccountTypeEnum)Enum.Parse(typeof(AccountTypeEnum), NewAccountType, true);
                                }
                                correctformat = true;
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid value for AccountType.");
                            }
                        }
                        #endregion

                        #region accone.CreationDate
                        Console.Write($"Enter the new Date.[old: {accone.CreationDate}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewCreationDate = Console.ReadLine();
                                if (true)
                                {
                                    accone.CreationDate = DateTime.Parse(NewCreationDate);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid date format.");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Date value is too large or too small.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message} Try another:");
                            }
                        }
                        #endregion

                        #region accone.Balance
                        Console.Write($"Enter the new Owner Account(AccountId).[old: {accone.Balance}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewBalance = Console.ReadLine();
                                if (NewBalance != "-")
                                {
                                    accone.Balance = int.Parse(NewBalance);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }
                        #endregion
                        rest.Put(accone, "account");
                        break;

                    case "Transaction":
                        #region Selecter
                        Console.Write("Enter Transaction's id to update: ");
                        correctformat = false;
                        int tranid = 0;
                        Transaction tranone = new Transaction();
                        while (!correctformat)
                        {
                            try
                            {
                                tranid = int.Parse(Console.ReadLine());
                                tranone = rest.Get<Transaction>(tranid, "transaction");
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Invalid format for TransactionId. Please enter a valid integer.");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Value for TransactionId is too large or too small for an integer.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}. Unexpected argument exception during API call.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Unexpected error: {ex.Message}");
                            }

                        }
                        #endregion

                        #region tranone.AccountId
                        Console.Write($"Enter the new Owner Account(AccountId).[old: {tranone.AccountId}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewAccountId = Console.ReadLine();
                                if (NewAccountId != "-")
                                {
                                    tranone.AccountId = int.Parse(NewAccountId);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine("The Input is incorrect: " + ex.Message + " Try Again!");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine("The Input is incorrect: " + ex.Message + " Try Again!");
                            }
                        }
                        #endregion
                        #region tranone.Id
                        Console.Write($"Enter the new Id.[old: {tranone.Id}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewId = Console.ReadLine();
                                if (NewId != "-")
                                {
                                    var asd = rest.Get<Transaction>(int.Parse(NewId), "transaction");

                                    rest.Get<Transaction>(tranid, "transaction");
                                    
                                    tranone.Id = int.Parse(NewId);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine("The Input is incorrect: " + ex.Message + " Try Again!");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine("The Input is incorrect: " + ex.Message + " Try Again!");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Error: {ex.Message} Try another:");
                            }
                        }
                        #endregion
                        #region tranone.Amount
                        Console.Write($"Enter the new Amount.[old: {tranone.Amount}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;

                        while (!correctformat)
                        {
                            try
                            {
                                string NewAmount = Console.ReadLine();
                                if (NewAmount != "-")
                                {
                                    tranone.Amount = int.Parse(NewAmount);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine("The Input is incorrect: " + ex.Message + " Try Again!");
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine("The Input is incorrect: " + ex.Message + " Try Again!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Fatal failure. Cause: {ex.Message}");
                            }
                        }
                        #endregion
                        #region tranone.Description
                        Console.Write($"Enter the new Description-.[old: {tranone.Description}] Write - if you dont want to modify.\nWrite here: ");
                        string NewDescription = Console.ReadLine();
                        if (NewDescription != "-")
                        {
                            tranone.Description = NewDescription;
                        }
                        #endregion
                        #region tranone.Date
                        Console.Write($"Enter the new Date.[old: {tranone.Date}] Write - if you dont want to modify.\nWrite here: ");
                        correctformat = false;
                        while (!correctformat)
                        {
                            try
                            {
                                string NewDateToTransfer = Console.ReadLine();
                                if (NewDateToTransfer != "-")
                                {
                                    tranone.Date = DateTime.Parse(NewDateToTransfer);
                                }
                                correctformat = true;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine("Wrong input! Try again! " + ex.Message);
                            }
                            catch (OverflowException ex)
                            {
                                Console.WriteLine("Wrong input! Try again! " + ex.Message);
                            }
                        }
                        #endregion
                        rest.Put(tranone, "Transaction");
                        break;

                    default:
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            Console.ReadLine();
        }
        static void Delete(string entity)
        {
            Console.WriteLine($"Which {entity} ID item do you want to delete?\nWrite here: ");

            try
            {
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
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"The removal failed. Cause: {ex.Message}");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"The removal failed. Cause: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"The removal failed. Cause: {ex.Message}");
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine("JSON deserialization error: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP request error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
            Console.ReadLine();
        }

        static void GetCustomersWithBirthdayInYear()
        {
            try
            {
                Console.WriteLine("Enter a birth year to find out you were born with it in a year.\nWrite here:");
                int BirthReq = 0;
                bool CoorrectFormat = false;
                while (!CoorrectFormat)
                {
                    try
                    {

                        BirthReq = int.Parse(Console.ReadLine());
                        CoorrectFormat = true;
                    }
                    catch (FormatException ex)
                    {

                        Console.WriteLine($"The removal failed. Cause: {ex.Message}\nTty Again!");
                    }
                    catch (OverflowException ex)
                    {

                        Console.WriteLine($"The removal failed. Cause: {ex.Message}\nTty Again!");
                    }

                }

                var Customers = rest.asd<Customer>(BirthReq, "api/NonCrud/GetCustomersWithBirthdayInYear");
                Console.WriteLine("{0,-10} |{1,-10} |{2,-24} |{3,-20} |{4,-16} |{5,-7}", "FirstName", "LastName", "Email", "Phone", "BirthDate", "Gender");
                Console.WriteLine(new string('-', 100));
                foreach (var item in Customers)
                {
                    Console.WriteLine("{0,-10} |{1,-10} |{2,-24} |{3,-20} |{4,-16} |{5,-7}", item.FirstName, item.LastName, item.Email, item.Phone, item.BirthDate.ToShortDateString(), item.Gender);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid argument: " + ex.Message);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine("JSON deserialization error: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP request error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
            Console.ReadLine();

        }

        static void GetCustomerTransactionInfo()
        {
            try
            {
                var Customers = rest.Get<CustomerTransactionInfo>("api/NonCrud/GetCustomerTransactionInfo");
                Console.WriteLine("{0,-10} |{1,-15} |{2,-15} |{3,-20}", "CustomerId", "FirstName", "LastName", "NumberOfTransactions");
                Console.WriteLine(new string('-', 100));
                foreach (var item in Customers)
                {
                    Console.WriteLine("{0,-10} |{1,-15} |{2,-15} |{3,-20}", item.CustomerId, item.FirstName, item.LastName, item.NumberOfTransactions);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid argument: " + ex.Message);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine("JSON deserialization error: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP request error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }

            Console.ReadLine();
        }

        static void GetCustomersWithAccountsAndTransactions()
        {
            try
            {
                var customerAccountInfo = rest.Get<CustomerAccountInfo>("api/NonCrud/GetCustomersWithAccountsAndTransactions");
                foreach (var info in customerAccountInfo)
                {
                    Console.WriteLine($"Ügyfél ID: {info.CustomerId}, Név: {info.FirstName} {info.LastName}");

                    foreach (var account in info.Accounts)
                    {
                        Console.WriteLine($"   Számla: {account.AccountNumber}, Tranzakciók száma: {account.TransactionCount}");
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid argument: " + ex.Message);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine("JSON deserialization error: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP request error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }

            Console.ReadLine();
        }

        static void GetCustomerTransactionDetails()
        {
            try
            {
                var customerTransactionDetails = rest.Get<CustomerTransactionDetails>("api/NonCrud/GetCustomerTransactionDetails");
                Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20}", "Customer Name", "Account Number", "Total Amount(Eur/Huf)", "Account Type");
                Console.WriteLine(new string('-', 60));

                foreach (var detail in customerTransactionDetails)
                {
                    string TAnountWithCurreny = detail.TotalTransactionAmount.ToString() + " " + detail.CurrencyType.ToString();
                    Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20}", detail.CustomerName, detail.Accountid, TAnountWithCurreny, detail.AccountType);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid argument: " + ex.Message);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine("JSON deserialization error: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP request error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }

            Console.ReadLine();
        }

        static void GetTotalSpendingLast30Days()
        {
            try
            {
                var totalSpendingLast30Days = rest.Get<CustomerTotalSpending>("/api/NonCrud/GetTotalSpendingLast30Days");
                Console.WriteLine("{0,-15} {1,-20} {2,-20}", "Ügyfél ID", "Ügyfél Név", "Összköltség (HUF)");
                Console.WriteLine(new string('-', 55));

                foreach (var entry in totalSpendingLast30Days)
                {
                    Console.WriteLine("{0,-15} {1,-20} {2,-20:C}", entry.CustomerId, entry.CustomerName, entry.TotalSpending);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid argument: " + ex.Message);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine("JSON deserialization error: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP request error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }

            Console.ReadLine();
        }

        static void GetLastIncomePerCustomer()
        {
            try
            {
                var lastNegativeTransactions = rest.Get<CustomerIncome>("api/NonCrud/GetLastIncomePerCustomer");
                foreach (var entry in lastNegativeTransactions)
                {
                    Console.WriteLine($"Ügyfél neve: {entry.CustomerName}, Utolsó negatív tranzakció összege: {entry.LastIncomeAmount} {entry.CurrencyType}");
                }

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid argument: " + ex.Message);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine("JSON deserialization error: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP request error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
            Console.ReadLine();
        }
    }
}