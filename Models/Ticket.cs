namespace BusStationWeb.Models
{
    // Проданый билет
    public class Ticket
    {
        public int TicketId { get; set; }
        public int TripId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string FIO { get; set; }
        public decimal Price { get; set; }
        public virtual Trip Trip { get; set; }
    }
}