using Microsoft.AspNetCore.Authorization;
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

        public IList<Models.Route> Routes { get; set; } = default!;
        public IList<Models.Trip> Trips { get; set; } = default!;
        public IList<Models.Ticket> Tickets { get; set; } = default!;
        public IList<Models.Citie> Cities { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Cities = await _context.Cities.ToListAsync();

            Routes = await _context.Routes.ToListAsync();
            
            Trips = await _context.Trips.Include(i => i.Route).ToListAsync();

            Tickets = await _context.Tickets.Include(i => i.Trip).Include(j => j.Trip.Route).ToListAsync();           
        }
    }
}