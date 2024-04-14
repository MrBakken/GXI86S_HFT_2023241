using GXI86S_HFT_2023241.Models;
using System;
using System.Linq;

namespace GXI86S_HFT_2023241.Repository
{
    public class AccountRepository : Repository<Account>, IRepository<Account>
    {
        public AccountRepository(BankDBContext ctx) : base(ctx)
        {
        }

        public override Account Read(int id)
        {
            return ctx.Accounts.FirstOrDefault(c => c.AccountNumber_ID == id);
        }

        public override void Update(Account item)
        {
            var old = Read(item.AccountNumber_ID);
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
