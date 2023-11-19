using GXI86S_HFT_2023241.Logic;
using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GXI86S_HFT_2023241.Test
{
    [TestFixture]
    public class CustumerLogicTester
    {
        private Mock<IRepository<Customer>> mockCustumerRepo;
        private Mock<IRepository<Account>> mockAccountRepo;
        private Mock<IRepository<Transaction>> mockTransactionRepo;
        private CustomerLogic logicC;
        private AccountLogic logicA;
        private TransactionLogic logicT;


        [SetUp]
        public void Init()
        {
            mockCustumerRepo = new Mock<IRepository<Customer>>();
            mockAccountRepo = new Mock<IRepository<Account>>();
            mockTransactionRepo = new Mock<IRepository<Transaction>>();
            logicC = new CustomerLogic(mockCustumerRepo.Object);
            logicA = new AccountLogic(mockAccountRepo.Object);
            logicT = new TransactionLogic(mockTransactionRepo.Object, mockAccountRepo.Object);


        }

        [Test]
        public void GetCustomerTransactionInfo_ShouldReturnCorrectTransactionInfo()
        {
            // Arrange
            var mockCustomerRepo = new Mock<IRepository<Customer>>();
            var customerLogic = new CustomerLogic(mockCustomerRepo.Object);

            // Előre definiált vásárlók és tranzakciók
            var customers = new List<Customer>
            {
                 new Customer { Id = 1, FirstName = "John", LastName = "Doe", Accounts = new List<Account> { new Account { Transactions = new List<Transaction> { new Transaction(), new Transaction(), new Transaction() } } } },
                 new Customer { Id = 2, FirstName = "Jane", LastName = "Doe", Accounts = new List<Account> { new Account {} } },
                 new Customer { Id = 3, FirstName = "Alice", LastName = "Smith", Accounts = new List<Account> { new Account { Transactions = new List<Transaction> { new Transaction(), new Transaction(), new Transaction(), new Transaction() } } } },
            };

            // Setup mock repository
            mockCustomerRepo.Setup(m => m.ReadAll()).Returns(customers.AsQueryable());

            // Act
            var result = customerLogic.GetCustomerTransactionInfo();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count()); // Három vásárló esetén
            Assert.AreEqual(3, result.First(info => info.CustomerId == 1).NumberOfTransactions); // Johnnak 3 tranzakciója van
            Assert.AreEqual(0, result.First(info => info.CustomerId == 2).NumberOfTransactions); // Jane-nek 2 tranzakciója van
            Assert.AreEqual(4, result.First(info => info.CustomerId == 3).NumberOfTransactions); // Alice-nek 4 tranzakciója van
        }

        
        [Test]
        public void GetCustomersWithAccountsAndTransactions_ShouldReturnCorrectAccountInfo()
        {
            // Arrange
            var mockCustomerRepo = new Mock<IRepository<Customer>>();
            var customerLogic = new CustomerLogic(mockCustomerRepo.Object);

            // Előre definiált vásárlók, számlák és tranzakciók
            var customers = new List<Customer>
    {
        new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Accounts = new List<Account>
            {
                new Account { AccountNumber_ID = 1, Transactions = new List<Transaction> { new Transaction(), new Transaction() } },
                new Account { AccountNumber_ID = 2, Transactions = new List<Transaction> { new Transaction(), new Transaction(), new Transaction() } }
            }
        },
        new Customer
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Doe",
            Accounts = new List<Account>
            {
                new Account { AccountNumber_ID = 3, Transactions = new List<Transaction> { new Transaction() } }
            }
        },
        new Customer
        {
            Id = 3,
            FirstName = "Alice",
            LastName = "Smith",
            Accounts = new List<Account>
            {
                new Account { AccountNumber_ID = 4, Transactions = new List<Transaction> { new Transaction(), new Transaction(), new Transaction(), new Transaction() } }
            }
        },
    };

            // Setup mock repository
            mockCustomerRepo.Setup(m => m.ReadAll()).Returns(customers.AsQueryable());

            // Act
            var result = customerLogic.GetCustomersWithAccountsAndTransactions();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count()); // Három vásárló esetén
            Assert.AreEqual(2, result.First(info => info.CustomerId == 1).Accounts.Count); // Johnnak két számlája van
            Assert.AreEqual(1, result.First(info => info.CustomerId == 2).Accounts.Count); // Jane-nek egy számlája van
            Assert.AreEqual(1, result.First(info => info.CustomerId == 3).Accounts.Count); // Alice-nek egy számlája van
        }

        [Test]
        public void GetCustomerTransactionDetails_ShouldReturnCorrectDetails()
        {
            // Arrange
            var mockCustomerRepo = new Mock<IRepository<Customer>>();
            var customerLogic = new CustomerLogic(mockCustomerRepo.Object);

            // Előre definiált vásárlók, számlák és tranzakciók
            var customers = new List<Customer>
    {
        new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Accounts = new List<Account>
            {
                new Account { AccountNumber_ID = 1, CurrencyType = CurrencyEnum.HUF, AccountType = AccountTypeEnum.Savings, Transactions = new List<Transaction> { new Transaction { Amount = 50 }, new Transaction { Amount = -30 } } },
                new Account { AccountNumber_ID = 2, CurrencyType = CurrencyEnum.EUR, AccountType = AccountTypeEnum.Current, Transactions = new List<Transaction> { new Transaction { Amount = -50 }, new Transaction { Amount = 20 } } }
            }
        },
        new Customer
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Doe",
            Accounts = new List<Account>
            {
                new Account { AccountNumber_ID = 3, CurrencyType = CurrencyEnum.HUF, AccountType = AccountTypeEnum.Savings, Transactions = new List<Transaction> { new Transaction { Amount = -100 }, new Transaction { Amount = 75 } } },
            }
        }
    };

            // Setup mock repository
            mockCustomerRepo.Setup(m => m.ReadAll()).Returns(customers.AsQueryable());

            // Act
            var result = customerLogic.GetCustomerTransactionDetails();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count()); // Összesen 6 tranzakció az előre definiált adatokban
            Assert.AreEqual("John Doe", result.First(details => details.Accountid == 1).CustomerName);
            Assert.AreEqual(50, result.First(details => details.Accountid == 1).TotalTransactionAmount);
            Assert.AreEqual(CurrencyEnum.HUF, result.First(details => details.Accountid == 1).CurrencyType);
            Assert.AreEqual(AccountTypeEnum.Savings, result.First(details => details.Accountid == 1).AccountType);
            // További ellenőrzések...
        }

        [Test]
        public void GetTotalSpendingLast30Days_ShouldReturnCorrectTotalSpending()
        {
            // Arrange
            var mockCustomerRepo = new Mock<IRepository<Customer>>();
            var customerLogic = new CustomerLogic(mockCustomerRepo.Object);

            // Előre definiált vásárlók, számlák és tranzakciók
            var customers = new List<Customer>
    {
        new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Accounts = new List<Account>
            {
                new Account
                {
                    AccountNumber_ID = 1,
                    CurrencyType = CurrencyEnum.HUF,
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Date = DateTime.Now.AddDays(-20), Amount = -50, Account = new Account{ CurrencyType = CurrencyEnum.HUF} },
                        new Transaction { Date = DateTime.Now.AddDays(-15), Amount = -30, Account = new Account{ CurrencyType = CurrencyEnum.HUF} },
                        new Transaction { Date = DateTime.Now.AddDays(-10), Amount = 20, Account = new Account{ CurrencyType = CurrencyEnum.HUF} },
                    }
                },
                new Account
                {
                    AccountNumber_ID = 2,
                    CurrencyType = CurrencyEnum.HUF,
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Date = DateTime.Now.AddDays(-25), Amount = -50, Account = new Account{ CurrencyType = CurrencyEnum.HUF} },
                        new Transaction { Date = DateTime.Now.AddDays(-20), Amount = -50, Account = new Account{ CurrencyType = CurrencyEnum.HUF} },
                        new Transaction { Date = DateTime.Now.AddDays(-30), Amount = -20, Account = new Account{ CurrencyType = CurrencyEnum.HUF} },
                        new Transaction { Date = DateTime.Now.AddDays(-31), Amount = -20, Account = new Account{ CurrencyType = CurrencyEnum.HUF} },
                    }
                }
            }
        },
        // Más vásárlók ...
    };

            // Setup mock repository
            mockCustomerRepo.Setup(m => m.ReadAll()).Returns(customers.AsQueryable());

            // Act
           
            var result = customerLogic.GetTotalSpendingLast30Days();
            ;
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count()); // Egy vásárló esetén
            Assert.AreEqual(-180, result.First().TotalSpending); // Johnnak két számlájának az elmúlt 30 napban történő összköltsége
        }






    }
}
