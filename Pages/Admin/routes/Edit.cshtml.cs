using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Routes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Route Route { get; set; } = default!;
        public List<SelectListItem> Cities { get; set; }

        public EditModel(Data.ApplicationDbContext context)
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
                Cities = _context.Cities.Select(c => new SelectListItem { Text = $"{c.NameCity}", Value = c.Id.ToString() }).ToList();
                var route = await _context.Routes.FirstOrDefaultAsync(r => r.RouteId == id);

                if (route == null)
                {
                    return NotFound();
                }
                Route = route;
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
                _context.Update(Route);
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