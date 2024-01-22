using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public Models.Contact Contact { get; set; }

        [BindProperty]
        public string CityName { get; set; }

        [BindProperty]
        public List<SelectListItem> Cities { get; set; }

        public EditModel(Data.ApplicationDbContext context)
        {
            _context = context;

            Cities = _context.Cities.Select(c => new SelectListItem { Text = $"{c.NameCity}", Value = c.Id.ToString() }).ToList();
            Cities.Add(new SelectListItem { Text = "Новое значение", Value = "0", Selected = false });
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
                var contact = await _context.Contacts.Include(x => x.Citie).FirstOrDefaultAsync(c => c.Id == id);

                if (contact == null)
                {
                    return NotFound();
                }
                Contact = contact;
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
                if (!string.IsNullOrEmpty(CityName) && Contact.CitieId == 0)
                {
                    Contact.Citie = new Models.Citie { NameCity = CityName };
                }

                _context.Update(Contact);
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