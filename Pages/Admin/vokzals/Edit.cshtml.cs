using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Vokzals
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        [BindProperty]
        public string TypeModel { get; set; }
        
        [BindProperty]
        public Models.Citie Citie { get; set; }

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