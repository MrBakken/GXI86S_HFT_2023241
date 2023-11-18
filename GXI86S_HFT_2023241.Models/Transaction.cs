using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GXI86S_HFT_2023241.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int? AccountId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Amount { get; set; }

        [StringLength(240)]
        public string Description { get; set; }

        public virtual Account Account { get; set; }

        public Transaction()
        {
        }

        public Transaction(string line)
        {
            string[] split = line.Split('$');
            Id = int.Parse(split[0]);
            AccountId = int.Parse(split[1]);
            Date = DateTime.Parse(split[2]);
            Amount = int.Parse(split[3]);
            Description = split[4];
            
        }

    }
    

}
