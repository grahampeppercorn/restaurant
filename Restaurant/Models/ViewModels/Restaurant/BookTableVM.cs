using Restaurant.Models.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models.ViewModels.Restaurant
{
    public class BookTableVM
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public int NumberOfPeople { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateForBooking { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime TimeOfDay { get; set; }
    }
}