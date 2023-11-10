using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using System;
using System.Linq;

namespace GXI86S_HFT_2023241.Logic
{
    public class BankLogic
    {
        IRepository<Customer> repo;

        public BankLogic(IRepository<Customer> repo)
        {
            this.repo = repo;
        }

        public void Create(Customer item)
        {
            int age = DateTime.Now.Year - item.BirthDate.Year;

            if (age <= 16)
            {
                throw new ArgumentException("The Cliet is too young...");
            }
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Customer Read(int id)
        {
            var custumer = this.repo.Read(id);
            if (custumer == null)
            {
                throw new ArgumentException("Client not exist...");
            }
            return this.repo.Read(id);
        }

        public IQueryable<Customer> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public void Update(Customer item)
        {
            this.repo.Update(item);
        }



    }
}
