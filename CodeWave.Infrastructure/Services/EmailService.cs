using CodeWave.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace CodeWave.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var host = _configuration["Email:SmtpHost"]!;
        var port = int.Parse(_configuration["Email:SmtpPort"]!);
        var senderEmail = _configuration["Email:SenderEmail"]!;
        var senderPassword = _configuration["Email:SenderPassword"]!;
        var senderName = _configuration["Email:SenderName"] ?? "CodeWave";

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(senderEmail, senderPassword)
        };

        var message = new MailMessage
        {
            From = new MailAddress(senderEmail, senderName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);

        await client.SendMailAsync(message);
    }
}
