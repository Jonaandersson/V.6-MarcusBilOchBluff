using System.Collections.Generic;
namespace MarcusBilOchBluffAB.Models
{
    public class ProfileViewModel
    {
        public List<Booking> UpcomingBookings { get; set; } = new();
        public List<Booking> PastBookings { get; set; } = new();
    }
}
