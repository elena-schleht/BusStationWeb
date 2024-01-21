using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Trips
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Trip Trip { get; set; }

        public DeleteModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id, string type)
        {
            TypeModel = type;

            if (type == "trip")
            {
                if (id == null || _context.Trips == null)
                {
                    return NotFound();
                }

                var trip = await _context.Trips.Include(x => x.Tickets).Include(x=>x.Route.From).Include(x => x.Route.To).FirstOrDefaultAsync(r => r.TripId == id);

                if (trip == null)
                {
                    return NotFound();
                }
                else
                {
                    Trip = trip;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string type)
        {
            TypeModel = type;

            if (type == "trip")
            {
                if (id == null || _context.Trips == null)
                {
                    return NotFound();
                }
                var trip = await _context.Trips.Include(x => x.Tickets).FirstOrDefaultAsync(x=>x.TripId == id);

                if (trip != null && trip.Tickets.Count == 0)
                {
                    Trip = trip;
                    _context.Trips.Remove(trip);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}