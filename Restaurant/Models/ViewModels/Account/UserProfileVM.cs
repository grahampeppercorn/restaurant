using Restaurant.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models.ViewModels.Account
{
    //basically the same as UserVM but passwords aren't required

    public class UserProfileVM
    {

        public UserProfileVM()
        {

        }

        public UserProfileVM(UserDTO row)
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
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
