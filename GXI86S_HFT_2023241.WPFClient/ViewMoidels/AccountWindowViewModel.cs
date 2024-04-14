using Castle.Core.Resource;
using GXI86S_HFT_2023241.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GXI86S_HFT_2023241.WPFClient.ViewMoidels
{
    public class AccountWindowViewModel : ObservableRecipient
    {
        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        public RestCollection<Account> Accounts { get; set; }

        private Account selectedAccount;

        public Account SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                if (value != null)
                {
                    selectedAccount = new Account()
                    {
                        AccountNumber_ID = value.AccountNumber_ID,
                        CustomerId = value.CustomerId,
                        CurrencyType = value.CurrencyType,
                        Balance = value.Balance,
                        CreationDate = value.CreationDate,
                        AccountType = value.AccountType,
                        Customer = value.Customer,
                        Transactions = value.Transactions,
                    };
                    OnPropertyChanged();
                    (DeleteAccountCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand CreateAccountCommand { get; set; }

        public ICommand DeleteAccountCommand { get; set; }

        public ICommand UpdateAccountCommand { get; set; }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }

        public AccountWindowViewModel()
        {
            if (!IsInDesignMode)
            {
                Accounts = new RestCollection<Account>("http://localhost:34372/", "account", "hub");

                CreateAccountCommand = new RelayCommand(() =>
                {
                    var customer = new Account()
                    {
                        CustomerId = selectedAccount.CustomerId,
                        CurrencyType = selectedAccount.CurrencyType,
                        AccountType = selectedAccount.AccountType,
                    };
                    Accounts.Add(customer);
                });


                UpdateAccountCommand = new RelayCommand(() =>
                {
                    try
                    {
                        Accounts.Update(SelectedAccount);
                    }
                    catch (ArgumentException ex)
                    {
                        ErrorMessage = ex.Message;
                    }

                });

                DeleteAccountCommand = new RelayCommand(() =>
                {
                    Accounts.Delete(SelectedAccount.AccountNumber_ID);
                    SelectedAccount = Accounts.First();
                },
                () =>
                {
                    return SelectedAccount != null;
                });
                SelectedAccount = new Account();
            }
        }
    }
}
