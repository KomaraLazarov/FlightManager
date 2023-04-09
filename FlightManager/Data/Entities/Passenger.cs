using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Data.Entities
{
    public class Passenger
    {
        public Passenger()
        {
            Reservations = new HashSet<Reservation>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SurName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "The Egn must be 10 digits", MinimumLength = 10)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter a valid Egn")]
        public string Egn { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "The phone number must be 10 digits", MinimumLength = 10)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string FullName 
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
