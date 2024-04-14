using GXI86S_HFT_2023241.WPFClient.Windows;
using System.Windows;
using System.Windows.Controls;

namespace GXI86S_HFT_2023241.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window a = new Window();
            if (sender is Button btn)
            {
                if (btn.Content.ToString() == "Customer")
                {
                    a = new CustomerWindow();
                }
                else if (btn.Content.ToString() == "Accounts")
                {
                    a = new AccountWindow();
                }
                else if (btn.Content.ToString() == "Transactions")
                {
                    a = new TransactionWindow();
                }
                else if (btn.Content.ToString() == "CRUD")
                {
                    a = new CrudWindow();
                }
                
                a.ShowDialog();
            }

        }
    }
}
