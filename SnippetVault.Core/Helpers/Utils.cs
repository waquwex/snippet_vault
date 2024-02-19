using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetVault.Core.Helpers
{
    public static class Utils
    {
        public static void ValidateModel(object? obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }

            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid)
            {
                var message = string.Join(" ", validationResults.Select(v =>
                {
                    var memberNames = string.Join(" , ", v.MemberNames);
                    return memberNames + " : " + v.ErrorMessage;
                }));
                throw new ValidationException(message);
            }
        }


        public static string TrimToLines(string text, int lines)
        {
            var output = string.Join("\r\n", text.Split("\r\n").Take(lines));
            
            return output;
        }
    }
}