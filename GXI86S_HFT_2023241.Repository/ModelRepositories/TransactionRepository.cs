using GXI86S_HFT_2023241.Models;
using System;
using System.Linq;

namespace GXI86S_HFT_2023241.Repository
{
    public class TransactionRepository : Repository<Transaction>, IRepository<Transaction>
    {
        public TransactionRepository(BankDBContext ctx) : base(ctx)
        {
        }

        public override Transaction Read(int id)
        {
            return ctx.Transactions.FirstOrDefault(c => c.Id == id);
        }

        public override void Update(Transaction item)
        {
            var old = Read(item.Id);
            if (old == null)
            {
                throw new ArgumentException("Item not exist..");
            }
            foreach (var prop in old.GetType().GetProperties())
            {
                if (prop.GetAccessors().FirstOrDefault(t => t.IsVirtual) == null)
                {
                    prop.SetValue(old, prop.GetValue(item));
                }
            }
            ctx.SaveChanges();
        }
    }
}
