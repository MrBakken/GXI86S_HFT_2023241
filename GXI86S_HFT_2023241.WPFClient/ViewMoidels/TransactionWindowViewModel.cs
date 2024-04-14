using GXI86S_HFT_2023241.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace GXI86S_HFT_2023241.WPFClient.ViewMoidels
{
    public class TransactionWindowViewModel : ObservableRecipient
    {
        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        public RestCollection<Transaction> Transactions { get; set; }

        private Transaction selectedTransaction;

        public Transaction SelectedTransaction
        {
            get { return selectedTransaction; }
            set
            {
                if (value != null)
                {
                    selectedTransaction = new Transaction()
                    {
                        Id = value.Id,
                        AccountId = value.AccountId,
                        Date = value.Date,
                        Amount = value.Amount,
                        Description = value.Description,
                    };
                    OnPropertyChanged();
                    (DeleteTransactionCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand CreateTransactionCommand { get; set; }

        public ICommand DeleteTransactionCommand { get; set; }

        public ICommand UpdateTransactionCommand { get; set; }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }

        public TransactionWindowViewModel()
        {
            if (!IsInDesignMode)
            {
                Transactions = new RestCollection<Transaction>("http://localhost:34372/", "transaction", "hub");

                CreateTransactionCommand = new RelayCommand(() =>
                {
                    var customer = new Transaction()
                    {
                        AccountId = selectedTransaction.AccountId,
                        Amount= selectedTransaction.Amount,
                        Description = selectedTransaction.Description,
                    };
                    Transactions.Add(customer);
                });


                UpdateTransactionCommand = new RelayCommand(() =>
                {
                    try
                    {
                        Transactions.Update(SelectedTransaction);
                    }
                    catch (ArgumentException ex)
                    {
                        ErrorMessage = ex.Message;
                    }

                });

                DeleteTransactionCommand = new RelayCommand(() =>
                {
                    Transactions.Delete(SelectedTransaction.Id);
                    SelectedTransaction = Transactions.First();
                },
                () =>
                {
                    return SelectedTransaction != null;
                });
                SelectedTransaction = new Transaction();
            }
        }
    }
}
