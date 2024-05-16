using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace karnaCrud.Models
{
    public class UserModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Only alphabetic characters are allowed in the name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Remote(action: "IsEmailAvailable", controller:"Home",ErrorMessage ="Email already exists!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [MaxLength(10, ErrorMessage = "Phone number must be at most 10 characters long")]
        [MinLength(10, ErrorMessage = "Phone number must be at least 10 characters long")]
        [Remote(action:"isPhoneAvailable",controller:"Home",ErrorMessage ="Phone number exists!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please select one gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(1000, ErrorMessage = "Address cannot exceed 1000 characters")]
        public string Address { get; set; }
    }
}
