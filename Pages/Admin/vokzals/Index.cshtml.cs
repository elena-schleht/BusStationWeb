using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Vokzals
{
    [Authorize]
    public class Index : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        public Index(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Contact> Contacts { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Contacts = await _context.Contacts.Include(x => x.Citie).ToListAsync();
        }
    }
}