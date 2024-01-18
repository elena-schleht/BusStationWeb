namespace BusStationWeb.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public int CitieId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WorkDays { get; set; }     
        public string Weekends { get; set; }

        public virtual Citie Citie { get; set; }
    }
}
