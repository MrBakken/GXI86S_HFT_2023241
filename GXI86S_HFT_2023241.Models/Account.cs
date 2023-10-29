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
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(240)]

        public string AccountNumber { get; set; }

        public int Balance { get; set; }

        public DateTime CreationDate { get; set; }

        public AccountType AccountType { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public Account()
        {
            Transactions = new List<Transaction>();
        }
        public Account(string line)
        {
            string[] split = line.Split('$');
            Id = int.Parse(split[0]);
            CustomerId = int.Parse(split[1]);
            AccountNumber = split[2];
            Balance = int.Parse(split[3]);
            CreationDate = DateTime.Parse(split[4]);

            Transactions = new List<Transaction>();

            if (Enum.TryParse<AccountType>(split[5], out AccountType temp))
            {
                AccountType = temp;
            }
        }
        
    }

    public enum AccountType
    {
        Current,
        Savings
    }
}
