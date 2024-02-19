
namespace SnippetVault.Core.ServiceContracts
{
    public interface IEmailService
    {
        Task SendAccountDeletedEmail(string email);
        Task SendConfirmationEmail(string confirmUrl, string email, string token);
    }
}