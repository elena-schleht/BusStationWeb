using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin
{
    public class AdminIndex : PageModel
    {
        private readonly BusStationWeb.Data.ApplicationDbContext _context;

        public AdminIndex(BusStationWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Route> Routes { get; set; } = default!;
        public IList<Models.Trip> Trips { get; set; } = default!;
        public IList<Models.Ticket> Tickets { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Routes = await _context.Routes.ToListAsync();
            
            Trips = await _context.Trips.Include(i => i.Route).ToListAsync();

            Tickets = await _context.Tickets.ToListAsync();
        }
    }
}