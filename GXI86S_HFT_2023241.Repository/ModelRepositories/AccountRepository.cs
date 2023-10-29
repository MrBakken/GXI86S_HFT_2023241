﻿using GXI86S_HFT_2023241.Models;
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
            return ctx.Accounts.FirstOrDefault(c => c.Id == id);
        }

        public override void Update(Account item)
        {
            var old = Read(item.Id);
            foreach (var prop in old.GetType().GetProperties())
            {
                prop.SetValue(old, prop.GetValue(item));
            }
            ctx.SaveChanges();
        }
    }
}
