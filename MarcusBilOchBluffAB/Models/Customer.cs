﻿namespace MarcusBilOchBluffAB.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }


}
