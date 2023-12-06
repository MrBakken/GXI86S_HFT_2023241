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

        [Test]
        public void GetLastIncomePerCustomer_ShouldReturnCorrectResult()
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
                        Transactions = new List<Transaction>
                        {
                            new Transaction { Date = DateTime.Now.AddDays(-20), Amount = -50, Account = new Account() },
                            new Transaction { Date = DateTime.Now.AddDays(-15), Amount = -30, Account = new Account() },
                            new Transaction { Date = DateTime.Now.AddDays(-10), Amount = 30, Account = new Account() },
                        }
                    },
                    new Account
                    {
                        AccountNumber_ID = 2,
                        Transactions = new List<Transaction>
                        {
                            new Transaction { Date = DateTime.Now.AddDays(-1), Amount = -50, Account = new Account() },
                            new Transaction { Date = DateTime.Now.AddDays(-20), Amount = 400, Account = new Account() },
                            new Transaction { Date = DateTime.Now.AddDays(-10), Amount = 20, Account = new Account() },
                        }
                    }
                }
            },
            // ... Egyéb ügyfelek
        };

            // Beállítás, hogy a mock repo visszaadja az előre definiált vásárlókat
            mockCustomerRepo.Setup(m => m.ReadAll()).Returns(customers.AsQueryable());

            // Act
            var result = customerLogic.GetLastIncomePerCustomer();
            ;
            // Assert
            Assert.AreEqual(1, result.Count()); // Ebben a példában 1 ügyfélnek van tranzakciója az előző 30 napban
            Assert.AreEqual(20, result.First().LastIncomeAmount);
        }




        [Test]
        public void Create_ThrowsException_WhenTransactionIsNotConnectedToAccount()
        {
            // Arrange
            var mockTransactionRepo = new Mock<IRepository<Transaction>>();
            var mockAccountRepo = new Mock<IRepository<Account>>();

            var transactionLogic = new TransactionLogic(mockTransactionRepo.Object, mockAccountRepo.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Trying to create a transaction without connecting it to an account
                transactionLogic.Create(new Transaction { /* Set other properties, but don't connect it to an account */ });
            });
        }

        [Test]
        public void Create_UpdatesAccountBalance_WhenTransactionIsConnectedToAccount()
        {
            // Arrange
            var mockTransactionRepo = new Mock<IRepository<Transaction>>();
            var mockAccountRepo = new Mock<IRepository<Account>>();

            var transactionLogic = new TransactionLogic(mockTransactionRepo.Object, mockAccountRepo.Object);

            var accountId = 1; // Set the account ID as needed

            var mockAccount = new Account
            {
                AccountNumber_ID = accountId,
                Balance = 100 // Set the initial balance as needed
            };

            mockAccountRepo.Setup(r => r.Read(accountId)).Returns(mockAccount);

            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = 50 // Set the transaction amount as needed
                            // Set other transaction properties as needed
            };

            // Act
            transactionLogic.Create(transaction);

            // Assert
            mockAccountRepo.Verify(r => r.Update(It.IsAny<Account>()), Times.Once); // Verifying that the Update method was called on the account repository
            Assert.AreEqual(150, mockAccount.Balance); // Verifying that the account balance was updated correctly
        }

        [Test]
        public void Read_ThrowsException_WhenTransactionDoesNotExist()
        {
            // Arrange
            var mockTransactionRepo = new Mock<IRepository<Transaction>>();
            var mockAccountRepo = new Mock<IRepository<Account>>();

            var transactionLogic = new TransactionLogic(mockTransactionRepo.Object, mockAccountRepo.Object);

            var nonExistentTransactionId = 1; // Set the non-existent transaction ID as needed
            mockTransactionRepo.Setup(r => r.Read(nonExistentTransactionId)).Returns((Transaction)null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Trying to read a non-existent transaction
                transactionLogic.Read(nonExistentTransactionId);
            });
        }

        [Test]
        public void Read_ReturnsTransaction_WhenTransactionExists()
        {
            // Arrange
            var mockTransactionRepo = new Mock<IRepository<Transaction>>();
            var mockAccountRepo = new Mock<IRepository<Account>>();

            var transactionLogic = new TransactionLogic(mockTransactionRepo.Object, mockAccountRepo.Object);

            var existingTransactionId = 1; // Set the existing transaction ID as needed
            var mockTransaction = new Transaction
            {
                // Set transaction properties as needed
            };

            mockTransactionRepo.Setup(r => r.Read(existingTransactionId)).Returns(mockTransaction);

            // Act
            var result = transactionLogic.Read(existingTransactionId);

            // Assert
            Assert.AreEqual(mockTransaction, result); // Verifying that the Read method returns the expected transaction
        }

        [Test]
        public void Create_ThrowsException_WhenNotConnectedToCustomer()
        {
            // Arrange
            var mockAccountRepo = new Mock<IRepository<Account>>();

            var accountLogic = new AccountLogic(mockAccountRepo.Object);

            var accountWithoutCustomer = new Account
            {
                // Set account properties as needed
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Trying to create an account without connecting it to a customer
                accountLogic.Create(accountWithoutCustomer);
            });
        }

        [Test]
        public void Create_CallsRepositoryCreate_WhenConnectedToCustomer()
        {
            // Arrange
            var mockAccountRepo = new Mock<IRepository<Account>>();

            var accountLogic = new AccountLogic(mockAccountRepo.Object);

            var customer = new Customer
            {
                // Set customer properties as needed
            };

            var accountConnectedToCustomer = new Account
            {
                Customer = customer,
                // Set account properties as needed
            };

            // Act
            accountLogic.Create(accountConnectedToCustomer);

            // Assert
            mockAccountRepo.Verify(r => r.Create(accountConnectedToCustomer), Times.Once);
        }
        [Test]
        public void Create_ThrowsException_WhenCustomerIsTooYoung()
        {
            // Arrange
            var mockCustomerRepo = new Mock<IRepository<Customer>>();

            var customerLogic = new CustomerLogic(mockCustomerRepo.Object);

            var youngCustomer = new Customer
            {
                // Set customer properties as needed, e.g., a birthdate for a person under 16
                BirthDate = DateTime.Now.AddYears(-15),
                FirstName = "Testas",
                LastName = "Testas",

            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Trying to create a customer who is too young
                customerLogic.Create(youngCustomer);
            });
        }

        [Test]
        public void Create_CallsRepositoryCreate_WhenCustomerIsOldEnough()
        {
            // Arrange
            var mockCustomerRepo = new Mock<IRepository<Customer>>();

            var customerLogic = new CustomerLogic(mockCustomerRepo.Object);

            var adultCustomer = new Customer
            {
                // Set customer properties as needed, e.g., a birthdate for a person 16 years or older
                BirthDate = DateTime.Now.AddYears(-20),
                FirstName = "Testas",
                LastName = "Testas",
            };

            // Act
            customerLogic.Create(adultCustomer);

            // Assert
            mockCustomerRepo.Verify(r => r.Create(adultCustomer), Times.Once);
        }
    }
}
