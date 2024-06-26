﻿using GXI86S_HFT_2023241.Models;
using System;
using System.Linq;

namespace GXI86S_HFT_2023241.Repository
{
    public class CustomerRepository : Repository<Customer>, IRepository<Customer>
    {
        public CustomerRepository(BankDBContext ctx) : base(ctx)
        {
        }

        public override Customer Read(int id)
        {
            return ctx.Customers.FirstOrDefault(c => c.Id == id);
        }

        public override void Update(Customer item)
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
