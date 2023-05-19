using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusStationWeb.Pages.Admin
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly BusStationWeb.Data.ApplicationDbContext _context;
        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Route Route { get; set; }
        [BindProperty]
        public Models.Trip Trip { get; set; }
        public List<SelectListItem> Routes { get; set; }


        public CreateModel(BusStationWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string type)
        {
            TypeModel = type;
            Routes = _context.Routes.Select(x => new SelectListItem { Text = $"{x.From} - {x.To}", Value = x.RouteId.ToString() }).ToList();
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