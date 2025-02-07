namespace MarcusBilOchBluffAB.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal PricePerDay { get; set; }
        public string ImagePath { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }


}
