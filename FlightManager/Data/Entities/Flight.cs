using FlightManager.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Data.Entities
{
    public class Flight
    {
        public Flight()
        {
            Reservations = new HashSet<Reservation>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string LocationFrom { get; set; }
        [Required]
        public string LocationTo { get; set; }
        [Required]
        public DateTime TakeOff { get; set; }
        [Required]
        [LandingTimeAttribute("TakeOff")]
        public DateTime Landing { get; set; }
        [Required]
        public string TypePlane { get; set; }
        [Required]
        public string UniCodePlane { get; set; }
        [Required]
        public string PilotName { get; set; }
        public int PassengerCapacity { get; set; }
        public int BusinessClassCapacity { get; set; }
        public string Duration
        {
            get
            {
                TimeSpan time = Landing.Subtract(TakeOff);
                return time.ToString();
            }
        }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
