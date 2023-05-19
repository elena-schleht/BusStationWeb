using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private readonly BusStationWeb.Data.ApplicationDbContext _context;
        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Route Route { get; set; }
        [BindProperty]
        public Models.Trip Trip { get; set; }

        public DeleteModel(BusStationWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id, string type)
        {
            TypeModel = type;

            if (id == null || _context.Routes == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.FirstOrDefaultAsync(m => m.RouteId == id);

            if (route == null)
            {
                return NotFound();
            }
            else
            {
                Route = route;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string type)
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

            return RedirectToPage("./AdminIndex");
        }
    }
}