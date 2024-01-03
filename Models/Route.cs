namespace BusStationWeb.Models
{
    // Маршрут
    public class Route
    {
        public int RouteId { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
        public virtual Citie From { get; set; }
        public virtual Citie To { get; set; }
    }
}