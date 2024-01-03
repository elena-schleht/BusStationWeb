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
        private readonly Data.ApplicationDbContext _context;

        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Citie Citie { get; set; }
        [BindProperty]
        public Models.Route Route { get; set; } = default!;
        [BindProperty]
        public Models.Trip Trip { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> Routes { get; set; }

        public EditModel(Data.ApplicationDbContext context)
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
                Cities = _context.Cities.Select(c => new SelectListItem { Text = $"{c.NameCity}", Value = c.Id.ToString() }).ToList();
                var route = await _context.Routes.FirstOrDefaultAsync(r => r.RouteId == id);

                if (route == null)
                {
                    return NotFound();
                }
                Route = route;
            }
            else
            {
                Routes = _context.Routes.Include(x => x.From).Include(x => x.To).Select(r => new SelectListItem { Text = $"{r.From.NameCity} - {r.To.NameCity}", Value = r.RouteId.ToString() }).ToList();

                if (id == null || _context.Trips == null)
                {
                    return NotFound();
                }

                var trip = await _context.Trips.FirstOrDefaultAsync(t => t.TripId == id);

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
            if (type == "citie")
            {
                _context.Update(Citie);
            }
            else if (type == "route")
            {
                _context.Update(Route);
            }
            else
            {
                _context.Update(Trip);
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