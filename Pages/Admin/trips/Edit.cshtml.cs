using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Trips
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Trip Trip { get; set; }
        public List<SelectListItem> Routes { get; set; }

        public EditModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id, string type)
        {
            TypeModel = type;

            
            if(TypeModel == "trip")
            {
                Routes = _context.Routes
                    .Include(x => x.From)
                    .Include(x => x.To)
                    .Select(r => new SelectListItem { Text = $"{r.From.NameCity} - {r.To.NameCity}", Value = r.RouteId.ToString() }).ToList();

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

            if (type == "trip")
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

            return RedirectToPage("./Index");
        }
    }
}