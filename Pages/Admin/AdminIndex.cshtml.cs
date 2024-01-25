using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin
{
    [Authorize]
    public class AdminIndex : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        public AdminIndex(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public JsonResult OnGetTicketsByMonth(DateTime dtStart, DateTime dtEnd)
        {
            var result = _context.Tickets
                .Where(x => x.PurchaseDate >= dtStart && x.PurchaseDate <= dtEnd).OrderBy(x=>x.PurchaseDate).ToList()
                .GroupBy(x => x.PurchaseDate.ToString("MMM yy"))
                .Select(group => new { Date = group.Key, Count = group.Count() }).ToList();

            return new JsonResult(result);
        }

        public JsonResult OnGetTicketsByRoute(DateTime dtStart, DateTime dtEnd)
        {
            var result = _context.Tickets.Include(x => x.Trip.Route.From).Include(x => x.Trip.Route.To)
                .Where(x => x.PurchaseDate >= dtStart && x.PurchaseDate <= dtEnd).OrderBy(x => x.PurchaseDate).ToList()
                .GroupBy(x => $"{x.Trip.Route.From.NameCity} - {x.Trip.Route.To.NameCity}")
                .Select(group => new { Date = group.Key, Count = group.Count() }).ToList();

            return new JsonResult(result);
        }
    }
}