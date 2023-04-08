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
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string LastName { get; set; }
        [StringLength(10, ErrorMessage = "The Egn must be 10 digits", MinimumLength = 10)]
        public string Egn { get; set; }
        [StringLength(10, ErrorMessage = "The phone number must be 10 digits", MinimumLength = 10)]
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
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
