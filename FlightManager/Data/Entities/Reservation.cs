using FlightManager.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Data.Entities
{
    public class Reservation
    {
        public Reservation()
        {
            Status = FlightStatusEnum.Pending;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FlightId { get; set; }
        public virtual Flight Flight { get; set; }
        public int PassengerId { get; set; }
        public virtual Passenger Passenger { get; set; }
        [Required]
        public TypeTicket TypeTicket { get; set; }
        public FlightStatusEnum Status { get; set; }
    }
}
