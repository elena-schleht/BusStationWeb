using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusStationWeb.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly BusStationWeb.Data.ApplicationDbContext _context;
        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Route Route { get; set; }
        [BindProperty]
        public Models.Trip Trip { get; set; }

        public CreateModel(BusStationWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string type)
        {
            TypeModel = type;
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