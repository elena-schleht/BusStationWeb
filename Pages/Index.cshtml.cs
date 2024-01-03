using BusStationWeb.Data;
using BusStationWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            //dbContext.Database.EnsureCreated();

            Trips = dbContext.Trips.Include(i => i.Route).Where(x => x.DepartureDate > DateTime.Now && x.AvailableSeats > 0).ToList();
        }

        public List<Trip> Trips { get; set; }

        public bool IsBooked { get; set; }
        public string Info { get; set; }
        public string Error { get; set; }


        public IActionResult OnPostTicket(string fio, int tripId)
        {
            var trip = Trips.Find(x => x.TripId == tripId);

            if (trip.AvailableSeats == 0)
            {
                Error = $"Извините свободных мест на данный рейс уже нет";
                return Page();
            }

            var exist = dbContext.Tickets.FirstOrDefault(x => x.Trip.TripId == trip.TripId
                && x.Trip.DepartureDate == trip.DepartureDate
                && x.FIO == fio);

            if (exist != null)
            {
                Error = $"Вы уже забронировали билет с номером <strong>{exist.TicketId}</strong>";
                return Page();
            }

            var newTicket = dbContext.Tickets.Add(new Ticket
            {
                TripId = trip.TripId,
                Price = trip.Route.Price,
                PurchaseDate = DateTime.Now,
                FIO = fio
            });
            trip.AvailableSeats -= 1;

            dbContext.SaveChanges();

            IsBooked = true;
            Info = $"Вы забронировали билет на <strong>{trip.DepartureDate} '{trip.Route.FromId} -> {trip.Route.ToId}'</strong>." +
                $"\r\nВаш уникальный номер <strong>{newTicket.Entity.TicketId}</strong> необходимо предъявить на кассе";

            return Page();
        }
    }
}