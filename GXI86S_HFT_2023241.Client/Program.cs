using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using System;
using System.IO;
using System.Linq;

namespace GXI86S_HFT_2023241.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRepository<Customer> repo = new CustomerRepository(new BankDBContext());

            Customer a = new Customer()
            {
                FirstName = "Béla"
            };

            repo.Create(a);

            var asd = repo.Read(1);
            asd.FirstName = "Sanyi";
            repo.Update(asd);

            var item =repo.ReadAll().ToArray();
            ;
        }
    }
}
