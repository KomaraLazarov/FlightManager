using FlightManager.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels
{
    public class ReservationViewModel
    {
        public int currentPageIndex { get; set; }
        public int pageCount { get; set; }

        public IEnumerable<Reservation> reservationList;
    }
}
