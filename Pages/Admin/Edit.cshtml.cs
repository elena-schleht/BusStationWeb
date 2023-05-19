using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly BusStationWeb.Data.ApplicationDbContext _context;

        [BindProperty]
        public string TypeModel { get; set; }

        [BindProperty]
        public Models.Route Route { get; set; } = default!;

        [BindProperty]
        public Models.Trip Trip { get; set; }
        public List<SelectListItem> Routes { get; set; }

        public EditModel(BusStationWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id, string type)
        {
            TypeModel = type;

            if (type == "route")
            {
                if (id == null || _context.Routes == null)
                {
                    return NotFound();
                }

                var route = await _context.Routes.FirstOrDefaultAsync(m => m.RouteId == id);

                if (route == null)
                {
                    return NotFound();
                }
                Route = route;
            }
            else
            {
                Routes = _context.Routes.Select(x => new SelectListItem { Text = $"{x.From} - {x.To}", Value = x.RouteId.ToString() }).ToList();

                if (id == null || _context.Trips == null)
                {
                    return NotFound();
                }

                var trip = await _context.Trips.FirstOrDefaultAsync(m => m.TripId == id);

                if (trip == null)
                {
                    return NotFound();
                }
                Trip = trip;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string type)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (type == "route")
            {
                _context.Attach(Route).State = EntityState.Modified;
            }
            else
            {
                _context.Attach(Trip).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./AdminIndex");
        }
    }
}