using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.Domain.IdentityEntities;
using SnippetVault.Core.Validators;
using System.ComponentModel.DataAnnotations;


namespace SnippetVault.Core.DTO.ApplicationUserDTOs
{
    public class RegisterDTO
    {
        [Required()]
        [StringLength(80, MinimumLength = 3)]
        [RegularExpression("^[A-Za-z\\d_\\-]*$", ErrorMessage = "Username field should contain alphanumeric and '_' '-' characters")]
        [Remote("IsUserNameNotRegistered", "Account", ErrorMessage = "User name is already taken")]
        public string? UserName { get; set; }

        [Required()]
        [EmailAddress]
        [Remote("IsEmailNotRegistered", "Account", ErrorMessage = "Email is already taken")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Password length should be in range of {2} - {1}")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[._\\-\\*])[A-Za-z\\d._\\-\\*]{8,}$",
            ErrorMessage = "Password field should contain atleast one upper case one lower case and one of '*' '_' '-' '.' characters")]
        [MaxRepeatingCharacters(3)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm password field is required")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm password should match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public ApplicationUser ToApplicationUser()
        {
            return new ApplicationUser()
            {
                Email = this.Email,
                UserName = this.UserName,
            };
        }
    }
}