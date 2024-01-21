using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages.Admin.Routes
{
    [Authorize]
    public class Index : PageModel
    {
        private readonly Data.ApplicationDbContext _context;

        public Index(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Route> Routes { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Routes = await _context.Routes.Include(x => x.To).Include(x => x.From).ToListAsync();
        }
    }
}