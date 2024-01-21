using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Tickets
{
    [Authorize]
    public class Index : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        public Index(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Ticket> Tickets { get; set; } = default!;
        public IList<Models.Citie> Cities { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Tickets = await _context.Tickets
                .Include(i => i.Trip.Route.From)
                .Include(i => i.Trip.Route.To).ToListAsync();           
        }
    }
}