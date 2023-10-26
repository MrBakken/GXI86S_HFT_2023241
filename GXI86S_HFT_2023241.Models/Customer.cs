using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

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

        [Required]
        [StringLength(240)]
        public string Address { get; set; }

        [StringLength(240)]
        public string Email { get; set; }

        [StringLength(240)]
        public string Phone { get; set; }

        public DateTime BirthDate { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }


    }

}
