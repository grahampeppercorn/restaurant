﻿using Restaurant.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models.ViewModels.Account
{
    public class UserVM
    {

        public UserVM()
        {

        }

        public UserVM(UserDTO row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            Surname = row.Surname;
            PhoneNumber = row.PhoneNumber;
            Email = row.Email;
            Username = row.Username;
            Password = row.Password;
        }

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
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}