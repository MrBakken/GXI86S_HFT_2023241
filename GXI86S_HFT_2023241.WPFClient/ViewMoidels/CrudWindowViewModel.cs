using GXI86S_HFT_2023241.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXI86S_HFT_2023241.WPFClient.ViewMoidels
{
    public class CrudWindowViewModel : ObservableRecipient
    {
        public RestCollection<CustomerTransactionInfo> CustomerTransactionInfos { get; set; } //CustomerTransactionInfo
        public RestCollection<CustomerAccountInfo> CustomerAccountInfos { get; set; }
        public RestCollection<CustomerTransactionDetails> CustomerTransactionDetails { get; set; }
        public RestCollection<CustomerTotalSpending> CustomerTotalSpendings { get; set; }
        public RestCollection<CustomerIncome> CustomerIncomes { get; set; }

        public CrudWindowViewModel() {
            CustomerTransactionInfos = new RestCollection<CustomerTransactionInfo>("http://localhost:34372/api/NonCrud/", "GetCustomerTransactionInfo");
            //CustomerAccountInfos = new RestCollection<CustomerAccountInfo>("http://localhost:34372/", "api/NonCrud/GetCustomersWithAccountsAndTransactions");
            //CustomerTransactionDetails = new RestCollection<CustomerTransactionDetails>("http://localhost:34372/", "api/NonCrud/GetCustomerTransactionDetails");
            //CustomerTotalSpendings = new RestCollection<CustomerTotalSpending>("http://localhost:34372/", "api/NonCrud/GetTotalSpendingLast30Days");
            //CustomerIncomes = new RestCollection<CustomerIncome>("http://localhost:34372/", "api/NonCrud/GetLastIncomePerCustomer");
        }

    }
}
