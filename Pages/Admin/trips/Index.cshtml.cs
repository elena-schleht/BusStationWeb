using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Trips
{
    [Authorize]
    public class Index : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        public Index(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Trip> Trips { get; set; } = default!;

        public async Task OnGetAsync()
        {          
            Trips = await _context.Trips.Include(i => i.Route.From).Include(i => i.Route.To).ToListAsync();
        }
    }
}