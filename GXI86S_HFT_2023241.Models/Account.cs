using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

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

        public decimal Balance { get; set; }

        public DateTime CreationDate { get; set; }

        public AccountType AccountType { get; set; }
    }
    public enum AccountType
    {
        Current,
        Savings
    }
}
