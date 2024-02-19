using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;


namespace SnippetVault.Core.Validators
{
    public class MaxRepeatingCharactersAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int _maxRepeatingCharacters;

        public MaxRepeatingCharactersAttribute(int maxRepeatingCharacters)
        {
            _maxRepeatingCharacters = maxRepeatingCharacters;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Null");
            }

            string inputPassword = (string) value;

            int repeatedCount = 0;

            for (int i = 0; i < inputPassword.Length; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                if (inputPassword[i] == inputPassword[i - 1])
                {
                    repeatedCount++;
                    if (repeatedCount > _maxRepeatingCharacters)
                    {
                        return new ValidationResult(GetErrorMessage(validationContext.MemberName));
                    }
                }
                else
                {
                    repeatedCount = 0;
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(string? memberName)
        {
            return string.Format(ErrorMessage ?? "{0} should not have repeating characters more than {1} times",
                            memberName ?? "Member", _maxRepeatingCharacters);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-max-repeating-characters", GetErrorMessage(context.ModelMetadata.PropertyName));
            context.Attributes.Add("data-val-max-repeating-characters-value", _maxRepeatingCharacters.ToString());
        }
    }
}
