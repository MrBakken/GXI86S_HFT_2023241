using GXI86S_HFT_2023241.Logic.InterfaceLogic;
using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using System;
using System.Linq;

namespace GXI86S_HFT_2023241.Logic
{
    public class AccountLogic : IAccountLogic
    {
        IRepository<Account> repo;

        public AccountLogic(IRepository<Account> repo)
        {
            this.repo = repo;
        }

        public void Create(Account item)
        {
            if (item.CustomerId != null || item.Customer != null)
            {
                item.Balance = 0;
                item.CreationDate = DateTime.Now;
                this.repo.Create(item); 
            }
            else
            {
                throw new ArgumentException("You have to connect it to Client...");
            }
        }

        public void Update(Account item)
        {
            this.repo.Update(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Account Read(int id)
        {
            var Account = this.repo.Read(id);
            return Account == null ? throw new ArgumentException("Account is not exist...") : this.repo.Read(id);
        }

        public IQueryable<Account> ReadAll()
        {
            return this.repo.ReadAll();
        }
    }
}
