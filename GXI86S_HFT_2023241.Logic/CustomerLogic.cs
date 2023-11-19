using GXI86S_HFT_2023241.Logic.InterfaceLogic;
using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GXI86S_HFT_2023241.Logic
{
    public class CustomerLogic : ICustomerLogic
    {
        IRepository<Customer> repo;

        public CustomerLogic(IRepository<Customer> repo)
        {
            this.repo = repo;
        }

        public void Create(Customer item)
        {
            int age = DateTime.Now.Year - item.BirthDate.Year;

            if (age <= 16)
            {
                throw new ArgumentException("The Cliet is too young...");
            }
            this.repo.Create(item);
        }

        public void Update(Customer item)
        {
            this.repo.Update(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Customer Read(int id)
        {
            var custumer = this.repo.Read(id);
            return custumer == null ? throw new ArgumentException("Client is not exist...") : this.repo.Read(id);
        }

        public IQueryable<Customer> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public IEnumerable<Customer> GetCustomersWithBirthdayInYear(int year)
        {
            var customersWithBirthday = this.repo.ReadAll()
                .Where(customer => customer.BirthDate.Year == year)
                .ToList();

            return customersWithBirthday;
        }

        public IEnumerable<CustomerTransactionInfo> GetCustomerTransactionInfo()
        {
            var customerTransactionInfo = this.repo.ReadAll()
                .Select(customer => new CustomerTransactionInfo
                {
                    CustomerId = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    NumberOfTransactions = customer.Accounts
                        .SelectMany(account => account.Transactions)
                        .Count()
                })
                .ToList();

            return customerTransactionInfo;
        }
        public class CustomerTransactionInfo
        {
            public int CustomerId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int NumberOfTransactions { get; set; }
        }


        public IEnumerable<CustomerAccountInfo> GetCustomersWithAccountsAndTransactions()
        {
            var customerAccountInfo = this.repo.ReadAll()
                .Where(customer => customer.Accounts.Any(account => account.Transactions.Any()))
                .Select(customer => new CustomerAccountInfo
                {
                    CustomerId = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Accounts = customer.Accounts
                        .Where(account => account.Transactions.Any())
                        .Select(account => new AccountInfo
                        {
                            AccountNumber = account.AccountNumber_ID,
                            TransactionCount = account.Transactions.Count()
                        })
                        .ToList()
                })
                .ToList();

            return customerAccountInfo;
        }
        public class CustomerAccountInfo
        {
            public int CustomerId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public List<AccountInfo> Accounts { get; set; }
        }

        public class AccountInfo
        {
            public int AccountNumber { get; set; }
            public int TransactionCount { get; set; }
        }

        public IEnumerable<CustomerTransactionDetails> GetCustomerTransactionDetails()
        {
            var query = this.repo.ReadAll()
                .SelectMany(customer => customer.Accounts
                    .SelectMany(account => account.Transactions
                        .Select(transaction => new CustomerTransactionDetails
                        {
                            CustomerName = $"{customer.FirstName} {customer.LastName}",
                            TotalTransactionAmount = (decimal)transaction.Amount,
                            CurrencyType = account.CurrencyType,
                            AccountType = account.AccountType,
                            Accountid = account.AccountNumber_ID
                        })
                    )
                );

            return query.ToList();
        }
        public class CustomerTransactionDetails
        {
            public string CustomerName { get; set; }
            public decimal TotalTransactionAmount { get; set; }
            public int Accountid { get; set; }
            public CurrencyEnum CurrencyType { get; set; }
            public AccountTypeEnum AccountType { get; set; }
        }

        public IEnumerable<CustomerTotalSpending> GetTotalSpendingLast30Days()
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            var asd = this.repo.ReadAll();
            ;
            var totalSpendingLast30Days = this.repo.ReadAll()
                .Select(customer => new CustomerTotalSpending
                {
                    CustomerId = customer.Id,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    TotalSpending = customer.Accounts
                        .SelectMany(account => account.Transactions)
                        .Where(transaction => transaction.Date >= thirtyDaysAgo && transaction.Amount < 0 )
                        .Sum(transaction => (decimal?)(Convertrer(transaction.Amount, transaction.Account.CurrencyType)) ?? 0)
                })
                .ToList();

            return totalSpendingLast30Days;
        }

        private decimal? Convertrer(double amount, CurrencyEnum currencyType)
        {
            double result = 0;
            double EurToHuf = 380;
            switch (currencyType)
            {
                case CurrencyEnum.EUR:
                    result = EurToHuf * amount;
                    break;
                case CurrencyEnum.HUF:
                    result = amount;
                    break;
                default:

                    break;
            }
            return (decimal?)result;
        }

        public class CustomerTotalSpending
        {
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public decimal TotalSpending { get; set; }
        }
        public IEnumerable<CustomerIncome> GetLastIncomePerCustomer()
        {
            return this.repo.ReadAll()
                .Select(customer => new
                {
                    Customer = customer,
                    LastNegativeTransaction = GetIncome(customer)
                })
                .Select(result => new CustomerIncome
                {
                    CustomerName = $"{result.Customer.FirstName} {result.Customer.LastName}",
                    LastIncomeAmount = result.LastNegativeTransaction != null ? (decimal)result.LastNegativeTransaction.Amount : 0,
                    CurrencyType = result.LastNegativeTransaction != null ? result.LastNegativeTransaction.Account.CurrencyType.ToString() : "Unknown"
                });
        }

        private static Transaction GetIncome(Customer customer)
        {
            return customer.Accounts
                .SelectMany(account => account.Transactions)
                .Where(transaction => transaction.Amount < 0)
                .OrderByDescending(transaction => transaction.Date)
                .FirstOrDefault();
        }

        public class CustomerIncome
        {
            public string CustomerName { get; set; }
            public decimal LastIncomeAmount { get; set; }
            public string CurrencyType { get; set; }
        }




    }
}

