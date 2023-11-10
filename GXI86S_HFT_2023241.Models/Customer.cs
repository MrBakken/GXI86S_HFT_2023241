using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GXI86S_HFT_2023241.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(240)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(240)]
        public string LastName { get; set; }

        [StringLength(240)]
        public string Email { get; set; }

        [StringLength(240)]
        public string Phone { get; set; }

        public DateTime BirthDate { get; set; }

        [StringLength(10)]
        public Genders Gender { get; set; }


        public virtual ICollection<Account> Accounts { get; set; }


        public Customer()
        {
            Accounts = new HashSet<Account>();
        }

        public Customer(string line)
        {
            string[] split = line.Split('$');
            Id = int.Parse(split[0]);
            FirstName = split[1];
            LastName = split[2];
            Email = split[3];
            Phone = split[4];
            BirthDate = DateTime.Parse(split[5].Replace("-", "."));
            Accounts = new HashSet<Account>();

            Genders temp;

            if (Enum.TryParse<Genders>(split[6], out temp))
            {
                Gender = temp;
            }

        }

    }
    public enum Genders
    {
        Male,
        Female
    }

}
