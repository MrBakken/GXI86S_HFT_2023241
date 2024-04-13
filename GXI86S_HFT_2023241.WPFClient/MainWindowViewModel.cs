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
using GXI86S_HFT_2023241.Models;
namespace GXI86S_HFT_2023241.WPFClient
{
    public class MainWindowViewModel : ObservableRecipient
    {
        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }


        public RestCollection<Customer> Customers { get; set; }

        private Customer selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                if (value != null)
                {
                    selectedCustomer = new Customer()
                    {
                        FirstName = value.FirstName,
                        Id = value.Id,

                    };
                    OnPropertyChanged();
                    (DeleteCustomerCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }


        public ICommand CreateCustomerCommand { get; set; }

        public ICommand DeleteCustomerCommand { get; set; }

        public ICommand UpdateCustomerCommand { get; set; }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }


        public MainWindowViewModel()
        {
            if (!IsInDesignMode)
            {
                Customers = new RestCollection<Customer>("http://localhost:34372/", "customer", "hub");
                CreateCustomerCommand = new RelayCommand(() =>
                {
                    Customers.Add(new Customer()
                    {
                        FirstName = selectedCustomer.FirstName,
                        LastName = "Valaki",
                        BirthDate = DateTime.Parse("1999.01.01"),
                        Gender = Genders.Male,
                        Phone = "",
                        Email = "",
                    });
                });

                UpdateCustomerCommand = new RelayCommand(() =>
                {
                    try
                    {
                        Customers.Update(SelectedCustomer);
                    }
                    catch (ArgumentException ex)
                    {
                        ErrorMessage = ex.Message;
                    }

                });

                DeleteCustomerCommand = new RelayCommand(() =>
                {
                    Customers.Delete(SelectedCustomer.Id);
                },
                () =>
                {
                    return SelectedCustomer != null;
                });
                SelectedCustomer = new Customer();
            }

        }
    }
}
