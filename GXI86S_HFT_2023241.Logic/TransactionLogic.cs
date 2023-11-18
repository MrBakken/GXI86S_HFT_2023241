using GXI86S_HFT_2023241.Logic.InterfaceLogic;
using GXI86S_HFT_2023241.Models;
using GXI86S_HFT_2023241.Repository;
using System;
using System.Linq;

namespace GXI86S_HFT_2023241.Logic
{
    public class TransactionLogic : ITransactionLogic
    {
        IRepository<Transaction> repo;

        public TransactionLogic(IRepository<Transaction> repo)
        {
            this.repo = repo;
        }

        public void Create(Transaction item)
        {
            if (item.AccountId != null || item.Account != null)
            {
                this.repo.Create(item);
                
            }
            else { 
                this.repo.Create(item); 
            }
            
        }

        public void Update(Transaction item)
        {
            this.repo.Update(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Transaction Read(int id)
        {
            var custumer = this.repo.Read(id);
            return custumer == null ? throw new ArgumentException("Account is not exist...") : this.repo.Read(id);
        }

        public IQueryable<Transaction> ReadAll()
        {
            return this.repo.ReadAll();
        }
    }
}
