using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarcusBilOchBluffAB.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        
        public bool IsConfirmed { get; set; }

        public IEnumerable<Car>? Cars { get; set; }
        public IEnumerable<Customer>? Customers { get; set; }
    }

}
