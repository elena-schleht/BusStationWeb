namespace BusStationWeb.Models
{
    //Рейс
    public class Trip
    {
        public int TripId { get; set; }
        public int RouteId { get; set; }
        public DateTime DepartureDate { get; set; }
        public int AvailableSeats { get; set; }
        public virtual Route Route { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}