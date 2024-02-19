using System.ComponentModel.DataAnnotations;

namespace SnippetVault.Core.DTO.ApplicationUserDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username or email field is required")]
        public string? FlexLoginName {
            get => flexLoginName;
            set
            {
                flexLoginName = value;
                var isEmail = flexLoginName.Contains("@");
                if (isEmail)
                {
                    Email = flexLoginName;
                }
                else
                {
                    UserName = flexLoginName;
                }
            }
        }

        private string? flexLoginName;

        [StringLength(80, MinimumLength = 3)]
        [RegularExpression("^[A-Za-z\\d_\\-]*$", ErrorMessage = "Username field should contain alphanumeric and '_' '-' characters")]
        public string? UserName { get; private set; }

        [EmailAddress]
        public string? Email { get; private set; }

        [Required(ErrorMessage = "Password field is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}