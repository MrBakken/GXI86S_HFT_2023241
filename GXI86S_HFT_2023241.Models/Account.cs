using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GXI86S_HFT_2023241.Models
{
    
    public class Account
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountNumber_ID { get; set; }

        public int? CustomerId { get; set; }

        [Required]
        public CurrencyEnum CurrencyType { get; set; }

        public int Balance { get; set; }

        public DateTime CreationDate { get; set; }

        public AccountTypeEnum AccountType { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public Account()
        {
            Transactions = new List<Transaction>();
        }
        public Account(string line)
        {
            string[] split = line.Split('$');
            AccountNumber_ID = int.Parse(split[0]);
            CustomerId = int.Parse(split[1]);

            if (Enum.TryParse<CurrencyEnum>(split[2], out CurrencyEnum temp))
            {
                CurrencyType = temp;
            }

            Balance = int.Parse(split[3]);
            CreationDate = DateTime.Parse(split[4]);

            if (Enum.TryParse<AccountTypeEnum>(split[5], out AccountTypeEnum temp2))
            {
                AccountType = temp2;
            }

            Transactions = new List<Transaction>();
        }

    }

    public enum AccountTypeEnum
    {
        Current,
        Savings
    }
    public enum CurrencyEnum
    {
        EURO,
        HUF
    }
}
