namespace BusStationWeb.Models
{
    // Маршрут
    public class Route
    {
        public int RouteId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
    }
}