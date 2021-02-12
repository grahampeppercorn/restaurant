using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models.DTOs
{
    public class BookTableDTO
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime DateForBooking { get; set; }
        public DateTime TimeOfDay { get; set; }
    }
}