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
        IRepository<Account> Arepo;

        public TransactionLogic(IRepository<Transaction> repo, IRepository<Account> Arepo)
        {
            this.repo = repo;
            this.Arepo = Arepo;
        }

        public void Create(Transaction item)
        {
            if (item.AccountId != null || item.Account != null)
            {
                Account ReadAcc = null;
                if (item.AccountId != null)
                {
                     ReadAcc = Arepo.Read((int)item.AccountId);
                }
                else
                {
                     ReadAcc = Arepo.Read(item.Account.AccountNumber_ID); 
                }
                item.Date = DateTime.Now;
                ReadAcc.Balance += item.Amount;
                Arepo.Update(ReadAcc); //nincsen tesztelve
                this.repo.Create(item);
            }
            else
            {
                throw new ArgumentException("You have to connect it to Account");
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
