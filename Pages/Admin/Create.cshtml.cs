using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin
{
    [Authorize]
    public class CreateModel : PageModel
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
        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> Routes { get; set; }


        public CreateModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string type)
        {
            TypeModel = type;
            Cities = _context.Cities.Select(c => new SelectListItem { Text = $"{c.NameCity}", Value = c.Id.ToString() }).ToList();
            Routes = _context.Routes.Include(x => x.From).Include(x => x.To).Select(r => new SelectListItem { Text = $"{r.From.NameCity} - {r.To.NameCity}", Value = r.RouteId.ToString() }).ToList();

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
                _context.Cities.Add(Citie);
            }
            else if (type == "route")
            {
                _context.Routes.Add(Route);
            }
            else
            {
                _context.Trips.Add(Trip);
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./AdminIndex");
        }
    }
}