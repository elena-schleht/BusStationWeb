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
        public Models.Contact Contact { get; set; }

        public DeleteModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id, string type)
        {
            TypeModel = type;

            if (type == "citie")
            {
                if (id == null || _context.Contacts == null)
                {
                    return NotFound();
                }
                var contact = await _context.Contacts.Include(x => x.Citie).FirstOrDefaultAsync(c => c.Id == id);

                if (contact == null)
                {
                    return NotFound();
                }
                Contact = contact;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string type)
        {
            TypeModel = type;

            if (type == "citie")
            {
                if (id == null || _context.Contacts == null)
                {
                    return NotFound();
                }
                var contact = await _context.Contacts.FindAsync(id);

                if (contact != null)
                {
                    _context.Contacts.Remove(contact);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}