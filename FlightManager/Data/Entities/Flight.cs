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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public DateTime TakeOff { get; set; }
        public DateTime Landing { get; set; }
        public string TypePlane { get; set; }
        public string UniCodePlane { get; set; }
        public string PilotName { get; set; }
        public int PassengerCapacity { get; set; }
        public int BusinessClassCapacity { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
