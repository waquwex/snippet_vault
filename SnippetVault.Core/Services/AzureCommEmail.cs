using SnippetVault.Core.ServiceContracts;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace SnippetVault.Core.Services
{
    public class AzureCommEmail : IEmailService
    {
        EmailClient _emailClient;
        string _sender;

        public AzureCommEmail(string connectionString, string sender)
        {
            _emailClient = new EmailClient(connectionString);
            _sender = sender;
        }

        public Task SendAccountDeletedEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task SendConfirmationEmail(string confirmUrl, string email, string token)
        {
            var url = $"{confirmUrl}?email={email}&token={token}";

            var subject = "Confirm E-Mail | SnippetVault";
            // put link into html
            var htmlContent = "<html>" +
                "<body>" +
                "<h1>SnippetVault Demo</h1>" +
                "<h4>Please confirm your email</h4>" +
                $"<a href=\"{url}\">Confirm</a>" +
                "</body>" +
                "</html>";
            var recipient = email;
            
            await _emailClient.SendAsync(Azure.WaitUntil.Started, _sender, recipient, subject, htmlContent);         
        }
    }
}
