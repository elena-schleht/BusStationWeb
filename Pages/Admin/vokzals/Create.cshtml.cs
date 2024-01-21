using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusStationWeb.Pages.Admin.Vokzals
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Citie Citie { get; set; }

        public CreateModel(Data.ApplicationDbContext context)
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

            if (type == "citie")
            {
                _context.Cities.Add(Citie);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}