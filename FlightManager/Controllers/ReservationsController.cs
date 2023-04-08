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
using System.IO;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace FlightManager.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservations.Include(r => r.Flight).Include(r => r.Passenger);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> MyReservations()
        {
            var applicationDbContext = _context.Reservations.Include(r => r.Flight).Include(r => r.Passenger);
            return View(await applicationDbContext.ToListAsync());
        }
        public IActionResult Confirm(int id)
        {

            var reservation = _context.Reservations.Include(r => r.Flight).Include(r => r.Passenger).FirstOrDefault(x=>x.Id  == id);
            Flight flight = reservation.Flight;

            if (reservation.TypeTicket == Data.Enums.TypeTicket.Business)
            {
                if (flight.BusinessClassCapacity - 1 >= 0)
                {
                    flight.BusinessClassCapacity -= 1;
                    reservation.Status = Data.Enums.FlightStatusEnum.Accepted;
                    _context.SaveChanges();

                    //string to = "niki04lazarov04@gmail.com"; //To address    
                    //string from = "niki04lazarov04@gmail.com"; //From address    
                    //MailMessage message = new MailMessage(from, to);

                    //string mailbody = $"You reserved 1 Business ticket. They left {flight.BusinessClassCapacity} tickets";
                    //message.Subject = "Sending Email Confirmation";
                    //message.Body = mailbody;
                    //message.BodyEncoding = Encoding.UTF8;
                    //message.IsBodyHtml = true;
                    //SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                    //System.Net.NetworkCredential basicCredential1 = new
                    //System.Net.NetworkCredential("niki04lazarov04@gmail.com", "nikolalazarov04");
                    //client.EnableSsl = true;
                    //client.UseDefaultCredentials = false;
                    //client.Credentials = basicCredential1;
                    //try
                    //{
                    //    client.Send(message);
                    //}

                    //catch (Exception ex)
                    //{
                    //    throw ex.InnerException;
                    //}
                    var email = new MimeMessage();

                    email.From.Add(new MailboxAddress("Sender Name", "niki04lazarov04@gmail.com"));
                    email.To.Add(new MailboxAddress("Receiver Name", "niki04lazarov04@gmail.com"));

                    email.Subject = "Testing out email sending";
                    email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                    {
                        Text = "Hello all the way from the land of C#"
                    };

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Connect("smtp.gmail.com", 587, false);

                        // Note: only needed if the SMTP server requires authentication
                        smtp.Authenticate("niki04lazarov04@gmail.com", "nikolalazarov04");

                        smtp.Send(email);
                        smtp.Disconnect(true);
                    }
                }
                else
                {
                    reservation.Status = Data.Enums.FlightStatusEnum.Declined;
                    _context.SaveChanges();
                }
            }
            else if (reservation.TypeTicket == Data.Enums.TypeTicket.Passenger)
            {
                if (flight.PassengerCapacity - 1 >= 0)
                {
                    flight.PassengerCapacity -= 1;
                    reservation.Status = Data.Enums.FlightStatusEnum.Accepted;
                    _context.SaveChanges();
                }
                else
                {
                    reservation.Status = Data.Enums.FlightStatusEnum.Declined;
                    _context.SaveChanges();
                }
            }


            return RedirectToAction(nameof(MyReservations));

        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Flight)
                .Include(r => r.Passenger)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id");
            ViewData["PassengerId"] = new SelectList(_context.Passenger, "Id", "Id");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlightId,PassengerId,TypeTicket,Status")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", reservation.FlightId);
            ViewData["PassengerId"] = new SelectList(_context.Passenger, "Id", "Id", reservation.PassengerId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", reservation.FlightId);
            ViewData["PassengerId"] = new SelectList(_context.Passenger, "Id", "Id", reservation.PassengerId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlightId,PassengerId,TypeTicket,Status")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Id", reservation.FlightId);
            ViewData["PassengerId"] = new SelectList(_context.Passenger, "Id", "Id", reservation.PassengerId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Flight)
                .Include(r => r.Passenger)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
