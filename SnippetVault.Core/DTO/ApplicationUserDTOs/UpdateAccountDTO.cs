using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.Validators;
using System.ComponentModel.DataAnnotations;


namespace SnippetVault.Core.DTO.ApplicationUserDTOs
{
    public class UpdateAccountDTO
    {
        [StringLength(80, MinimumLength = 3)]
        [RegularExpression("^[A-Za-z\\d_\\-]*$", ErrorMessage = "Username field should contain alphanumeric and '_' '-' characters")]
        [Remote("IsUserNameNotRegistered", "Account", ErrorMessage = "User name is already taken")]
        public string? UserName { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "Current password field is required")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password field is required")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Password length should be in range of {2} - {1}")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[._\\-\\*])[A-Za-z\\d._\\-\\*]{8,}$",
            ErrorMessage = "Password field should contain atleast one upper case one lower case and one of '*' '_' '-' '.' characters")]
        [MaxRepeatingCharacters(3)]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password field is required")]
        [Compare(nameof(NewPassword), ErrorMessage = "Password and confirm password should match")]
        [DataType(DataType.Password)]
        public string? ConfirmNewPassword { get; set; }
    }
}
