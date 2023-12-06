using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXI86S_HFT_2023241.Models
{


    #region FORnoncrud


    public class CustomerAccountInfo
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<AccountInfo> Accounts { get; set; }
    }

    public class CustomerTransactionInfo
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfTransactions { get; set; }
    }

    public class AccountInfo
    {
        public int AccountNumber { get; set; }
        public int TransactionCount { get; set; }
    }

    public class CustomerTransactionDetails
    {
        public string CustomerName { get; set; }
        public decimal TotalTransactionAmount { get; set; }
        public int Accountid { get; set; }
        public CurrencyEnum CurrencyType { get; set; }
        public AccountTypeEnum AccountType { get; set; }
    }

    

    public class CustomerTotalSpending
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalSpending { get; set; }
    }

    

    public class CustomerIncome
    {
        public string CustomerName { get; set; }
        public decimal LastIncomeAmount { get; set; }
        public string CurrencyType { get; set; }
    }
    #endregion

}
