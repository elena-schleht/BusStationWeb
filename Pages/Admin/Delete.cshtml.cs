using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Citie Citie { get; set; }
        [BindProperty]
        public Models.Route Route { get; set; }
        [BindProperty]
        public Models.Trip Trip { get; set; }

        public DeleteModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id, string type)
        {
            TypeModel = type;

            if (type == "citie")
            {
                if (id == null || _context.Cities == null)
                {
                    return NotFound();
                }
                var citie = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);

                if (citie == null)
                {
                    return NotFound();
                }
                Citie = citie;
            }
            else if (type == "route")
            {
                if (id == null || _context.Routes == null)
                {
                    return NotFound();
                }

                var route = await _context.Routes.Include(x=>x.From).Include(x => x.To).FirstOrDefaultAsync(r => r.RouteId == id);

                if (route == null)
                {
                    return NotFound();
                }
                else
                {
                    Route = route;
                }
            }
            else
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

            if (type == "citie")
            {
                if (id == null || _context.Cities == null)
                {
                    return NotFound();
                }
                var citie = await _context.Cities.FindAsync(id);

                if (citie != null)
                {
                    Citie = citie;
                    _context.Cities.Remove(Citie);
                    await _context.SaveChangesAsync();
                }
            }
            else if (type == "route")
            {
                if (id == null || _context.Routes == null)
                {
                    return NotFound();
                }
                var route = await _context.Routes.FindAsync(id);

                if (route != null)
                {
                    Route = route;
                    _context.Routes.Remove(Route);
                    await _context.SaveChangesAsync();
                }
            }
            else
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

            return RedirectToPage("./AdminIndex");
        }
    }
}