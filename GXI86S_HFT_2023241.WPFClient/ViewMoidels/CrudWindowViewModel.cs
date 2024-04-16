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
        public List<CustomerTransactionInfo> CustomerTransactionInfos { get; set; } //CustomerTransactionInfo
        public List<CustomerAccountInfo> CustomerAccountInfos { get; set; }
        public List<CustomerTransactionDetails> CustomerTransactionDetails { get; set; }
        public List<CustomerTotalSpending> CustomerTotalSpendings { get; set; }
        public List<CustomerIncome> CustomerIncomes { get; set; }

        public CrudWindowViewModel() {

            var noncrud = new RestService("http://localhost:34372/");
            CustomerTransactionInfos = noncrud.Get<CustomerTransactionInfo>("api/NonCrud/GetCustomerTransactionInfo");
            CustomerAccountInfos = noncrud.Get<CustomerAccountInfo>("api/NonCrud/GetCustomersWithAccountsAndTransactions");
            CustomerTransactionDetails = noncrud.Get<CustomerTransactionDetails>("api/NonCrud/GetCustomerTransactionDetails");
            CustomerTotalSpendings = noncrud.Get<CustomerTotalSpending>("api/NonCrud/GetTotalSpendingLast30Days");
            CustomerIncomes = noncrud.Get<CustomerIncome>("api/NonCrud/GetLastIncomePerCustomer");

            //CustomerAccountInfos = new RestCollection<CustomerAccountInfo>("http://localhost:34372/", "api/NonCrud/GetCustomersWithAccountsAndTransactions");
            //CustomerTransactionDetails = new RestCollection<CustomerTransactionDetails>("http://localhost:34372/", "api/NonCrud/GetCustomerTransactionDetails");
            //CustomerTotalSpendings = new RestCollection<CustomerTotalSpending>("http://localhost:34372/", "api/NonCrud/GetTotalSpendingLast30Days");
            //CustomerIncomes = new RestCollection<CustomerIncome>("http://localhost:34372/", "api/NonCrud/GetLastIncomePerCustomer");
        }

    }
}
