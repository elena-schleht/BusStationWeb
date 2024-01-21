using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Routes
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Route Route { get; set; }

        public DeleteModel(Data.ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string type)
        {
            TypeModel = type;

            if (type == "route")
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

            return RedirectToPage("./Index");
        }
    }
}