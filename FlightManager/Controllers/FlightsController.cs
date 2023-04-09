using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightManager.Data;
using FlightManager.Data.Entities;
using FlightManager.ViewModels;

namespace FlightManager.Controllers
{
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flights
        public IActionResult Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                GetFlightList(1).flightList = GetFlightList(1).flightList.Where(s => s.LocationFrom.Contains(searchString));
            }

            return View(GetFlightList(1));
        }

        [HttpPost]
        public IActionResult Index(int currentPageIndex)
        {
            return View(GetFlightList(currentPageIndex));
        }
        public IActionResult AllPassengers(int id)
        {
            var flight = _context.Flights.FirstOrDefault(x=>x.Id == id);

            var reservation = _context.Reservations.Include(r => r.Flight).Include(r => r.Passenger).FirstOrDefault(x => x.FlightId == flight.Id);

            var allPassengers = _context.Passenger.ToList();

            List<Passenger> passengersForAFlight = new List<Passenger>();

            try
            {
                foreach (var passenger in allPassengers.Where(x => x.Id == reservation.Passenger.Id))
                {
                    if (passenger != null)
                    {
                        passengersForAFlight.Add(passenger);
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }


            return View(passengersForAFlight.ToList());
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LocationFrom,LocationTo,TakeOff,Landing,TypePlane,UniCodePlane,PilotName,PassengerCapacity,BusinessClassCapacity")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LocationFrom,LocationTo,TakeOff,Landing,TypePlane,UniCodePlane,PilotName,PassengerCapacity,BusinessClassCapacity")] Flight flight)
        {
            if (id != flight.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.Id == id);
        }

        private FlightViewModel GetFlightList(int currentPage)
        {
            int maxRowsPerPAge = 10;
            FlightViewModel flightModel = new FlightViewModel();

            flightModel.flightList = _context.Flights
                .Skip((currentPage - 1) * maxRowsPerPAge).Take(maxRowsPerPAge).ToList();

            double pageCount = (double)((decimal)_context.Flights.Count() / Convert.ToDecimal(maxRowsPerPAge));

            flightModel.pageCount = (int)Math.Ceiling(pageCount);
            flightModel.currentPageIndex = currentPage;

            return flightModel;
        }
    }
}
