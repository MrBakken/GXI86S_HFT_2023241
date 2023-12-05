using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GXI86S_HFT_2023241.Models
{
    
    public class Account
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountNumber_ID { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CurrencyEnum CurrencyType { get; set; }

        public double Balance { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountTypeEnum AccountType { get; set; }

        public virtual Customer Customer { get; set; }

        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions { get; set; }

        public Account()
        {
            Transactions = new HashSet<Transaction>();
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
        EUR,
        HUF
    }
}
