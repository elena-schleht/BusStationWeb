using BusStationWeb.Data;
using BusStationWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusStationWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            //Date = DateTime.Now.Date.ToString("dd.MM.yyyy");
            Routes = dbContext.Routes.Include(r => r.From).Include(r => r.To).ToList();
            Trips = dbContext.Trips.Include(i => i.Route).Where(x => /*x.DepartureDate > DateTime.Now &&*/ x.AvailableSeats > 0).ToList();
        }

        public List<Models.Route> Routes { get; set; }
        public List<Trip> Trips { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public string Date { get; set; }

        public bool IsBooked { get; set; }
        public string Info { get; set; }
        public string Error { get; set; }


        public async Task<PartialViewResult> OnGetFilterAsync(DateTime filterDate)
        {
            Routes = await dbContext.Routes.Include(r => r.From).Include(r => r.To).ToListAsync();
            Trips = await dbContext.Trips.Include(i => i.Route).Where(x => /*x.DepartureDate > DateTime.Now &&*/ x.DepartureDate.Date == filterDate && x.AvailableSeats > 0).ToListAsync();
            return Partial("TripsPartial", Trips);
        }

        public IActionResult OnPostTicket(int tripId)
        {
            var trip = Trips.Find(x => x.TripId == tripId);

            if (trip.AvailableSeats == 0)
            {
                Error = $"Извините свободных мест на данный рейс уже нет";
                return Page();
            }

            var newTicket = dbContext.Tickets.Add(new Ticket
            {
                TripId = trip.TripId,
                Price = trip.Route.Price,
                PurchaseDate = DateTime.Now
            });
            trip.AvailableSeats -= 1;

            dbContext.SaveChanges();

            IsBooked = true;
            Info = $"Вы забронировали билет на <strong>{trip.DepartureDate} '{trip.Route.From.NameCity} -> {trip.Route.To.NameCity}'</strong>." +
                $"\r\nВаш уникальный номер <strong>{newTicket.Entity.TicketId}</strong> необходимо предъявить на кассе";

            return Page();
        }
    }
}