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

        public IList<Models.Citie> Cities { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Cities = await _context.Cities.ToListAsync();
        }
    }
}