using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using GXI86S_HFT_2023241.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace GXI86S_HFT_2023241.WPFClient.ViewMoidels
{
    public class CustomerWindowViewModel : ObservableRecipient
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
                        LastName = value.LastName,
                        Gender = value.Gender,
                        BirthDate = value.BirthDate,
                        Email = value.Email,
                        Phone = value.Phone,
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


        public CustomerWindowViewModel()
        {
            if (!IsInDesignMode)
            {
                Customers = new RestCollection<Customer>("http://localhost:34372/", "customer", "hub");
                CreateCustomerCommand = new RelayCommand(() =>
                {
                    var customer = new Customer()
                    {
                        FirstName = selectedCustomer.FirstName,
                        LastName = selectedCustomer.LastName,
                        BirthDate = selectedCustomer.BirthDate,
                        Gender = selectedCustomer.Gender,
                        Phone = selectedCustomer.Phone,
                        Email = selectedCustomer.Email
                    };
                    Customers.Add(customer);
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
                    SelectedCustomer = Customers.First();
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
