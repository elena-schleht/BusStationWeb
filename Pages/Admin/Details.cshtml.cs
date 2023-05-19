using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly BusStationWeb.Data.ApplicationDbContext _context;
        
        public Models.Ticket Ticket { get; set; }

        public DetailsModel(BusStationWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var route = await _context.Tickets.FirstOrDefaultAsync(m => m.TicketId == id);
            if (route == null)
            {
                return NotFound();
            }
            else
            {
                Ticket = route;
            }
            return Page();
        }
    }
}