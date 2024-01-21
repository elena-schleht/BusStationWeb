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
        }

        [BindProperty, DataType(DataType.Date)]
        public DateTime Date { get; set; }        

        public async Task<PartialViewResult> OnGetTripsAsync(DateTime filterDate)
        {
            var trips = await dbContext.Trips
                .Include(x => x.Route.From)
                .Include(x => x.Route.To)
                .Where(x => /*x.DepartureDate > DateTime.Now &&*/ x.DepartureDate.Date == filterDate && x.AvailableSeats > 0)
                .ToListAsync();
            return Partial("TripsPartial", trips);
        }

        public async Task<PartialViewResult> OnPostBook(int tripId, DateTime filterDate)
        {
            var trip = await dbContext.Trips
                .Include(x => x.Route.From)
                .Include(x => x.Route.To)
                .SingleAsync(x => x.TripId == tripId);

            var alert = new Alert();

            if (trip.AvailableSeats == 0)
            {
                alert.Error = $"Извините свободных мест на данный рейс уже нет";
            }
            else
            {
                var newTicket = dbContext.Tickets.Add(new Ticket
                {
                    TripId = trip.TripId,
                    Price = trip.Route.Price,
                    PurchaseDate = DateTime.Now
                });
                trip.AvailableSeats -= 1;

                dbContext.SaveChanges();

                alert.Info = $"Вы забронировали билет на <strong>{trip.DepartureDate} '{trip.Route.From.NameCity} -> {trip.Route.To.NameCity}'</strong>." +
                    $"\r\nВаш уникальный номер <strong>{newTicket.Entity.TicketId}</strong> необходимо предъявить на кассе";
            }

            var trips = await dbContext.Trips
                .Include(x => x.Route.From)
                .Include(x => x.Route.To)
                .Where(x => /*x.DepartureDate > DateTime.Now &&*/ x.DepartureDate.Date == filterDate && x.AvailableSeats > 0)
                .ToListAsync();

            return Partial("AlertPartial", alert);
        }
    }
}