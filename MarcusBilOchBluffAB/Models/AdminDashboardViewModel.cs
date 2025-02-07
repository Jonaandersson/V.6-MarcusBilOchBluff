namespace MarcusBilOchBluffAB.Models
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<Car> Cars { get; set; } = new List<Car>();
        public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();
        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();
    }
}
