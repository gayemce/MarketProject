using GenericEmailService;
using MarketServer.WebApi.Options;
using MarketServer.WebApi.Utilities;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace MarketServer.WebApi.Services;

public static class MailService
{
    public static async Task<string> SendEmailAsync(string email, string subject, string body)
    {
        var emailSettings = ServiceTool.ServiceProvider.GetRequiredService<IOptions<EmailSettings>>();

        /* GenericEmailService 1.0.3 Kütüphanesi kullanılarak mail gönderme işleminin ayarları yapıldı. */

        EmailConfigurations configurations = new(
                                        Smtp: emailSettings.Value.SMTP,
                                        Password: emailSettings.Value.Password,
                                        Port: emailSettings.Value.Port,
                                        SSL: emailSettings.Value.SSL,
                                        Html: true);

        List<string> emails = new() { email }; //Mail gönderilecek kişiler listesi

        EmailModel<Stream> model = new(
                                  Configurations: configurations,
                                  FromEmail: emailSettings.Value.Email,
                                  ToEmails: emails,
                                  Subject: subject,
                                  Body: body,
                                  Attachments: null);

        string response = await EmailService.SendEmailWithMailKitAsync(model);
        return response;
    }
}
