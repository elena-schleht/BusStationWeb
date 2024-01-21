using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Vokzals
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        [BindProperty]
        public string TypeModel { get; set; }
        [BindProperty]
        public Models.Citie Citie { get; set; }

        public DeleteModel(Data.ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string type)
        {
            TypeModel = type;

            if (type == "citie")
            {
                if (id == null || _context.Cities == null)
                {
                    return NotFound();
                }
                var citie = await _context.Cities.FindAsync(id);

                if (citie != null)
                {
                    Citie = citie;
                    _context.Cities.Remove(Citie);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}