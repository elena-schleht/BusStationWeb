using BusStationWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BusStationWeb.Pages
{
    public class ContactsModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public List<Models.Contact> Contacts { get; set; }

        public ContactsModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void OnGet()
        {
            Contacts = dbContext.Contacts.Include(x=>x.Citie).ToList();
        }
    }
}
