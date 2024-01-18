using BusStationWeb.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusStationWeb.Pages
{
    public class TripsPartialModel : PageModel
    {
        public List<Trip> Trips { get; set; }
    }
}
