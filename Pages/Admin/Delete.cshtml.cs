using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusStationWeb.Data;
using BusStationWeb.Models;

namespace BusStationWeb.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private readonly BusStationWeb.Data.ApplicationDbContext _context;

        public DeleteModel(BusStationWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Models.Route Route { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Routes == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.FirstOrDefaultAsync(m => m.RouteId == id);

            if (route == null)
            {
                return NotFound();
            }
            else 
            {
                Route = route;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Routes == null)
            {
                return NotFound();
            }
            var route = await _context.Routes.FindAsync(id);

            if (route != null)
            {
                Route = route;
                _context.Routes.Remove(Route);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./AdminIndex");
        }
    }
}
