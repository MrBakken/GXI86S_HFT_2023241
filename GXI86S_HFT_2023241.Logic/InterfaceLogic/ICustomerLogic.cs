using GXI86S_HFT_2023241.Models;
using System.Collections.Generic;
using System.Linq;

namespace GXI86S_HFT_2023241.Logic.InterfaceLogic
{
    public interface ICustomerLogic
    {
        void Create(Customer item);
        void Delete(int id);
        IEnumerable<CustomerLogic.CustomerAccountInfo> GetCustomersWithAccountsAndTransactions();
        IEnumerable<Customer> GetCustomersWithBirthdayInYear(int year);
        IEnumerable<CustomerLogic.CustomerTransactionDetails> GetCustomerTransactionDetails();
        IEnumerable<CustomerLogic.CustomerTransactionInfo> GetCustomerTransactionInfo();
        IEnumerable<CustomerLogic.CustomerIncome> GetLastIncomePerCustomer();
        IEnumerable<CustomerLogic.CustomerTotalSpending> GetTotalSpendingLast30Days();
        Customer Read(int id);
        IQueryable<Customer> ReadAll();
        void Update(Customer item);
    }
}