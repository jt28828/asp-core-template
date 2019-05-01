using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using RazorEngine;
using RazorEngine.Templating;

namespace DotnetCoreWebApiTemplate.Services
{
    /// <summary>
    /// A Singleton service used to send emails to users
    /// </summary>
    public class MailerService
    {
        private readonly string _emailAddress;
        private readonly SmtpClient _mailServer;

        public MailerService(string host, string port, string username, string password)
        {
            int portAsInt = int.Parse(port);
            _mailServer = new SmtpClient(host, portAsInt)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password)
            };
            _emailAddress = username;
        }

        /// <summary> A Demo mailing function </summary>
        public async void MailFunction(string text, string recipientsName)
        {
            string templatePath =
                Path.Combine(Environment.CurrentDirectory, @"wwwroot/Templates/Email/email-example.cshtml");
            string templateText = await File.ReadAllTextAsync(templatePath);

            // The model to pass to Razor for compiling the template. Probably use a non anonymous object
            var templateDemoModel = new {Name = recipientsName, Text = text};


            // You will end up with the string of the template with the variables added into the HTML
            string messageBody = Engine.Razor.RunCompile(templateText, "demoTemplate",
                templateDemoModel.GetType(), templateDemoModel);

            // Alternatively to use a non-anonymous type use this syntax
            // string messageBody = Engine.Razor.RunCompile(templateText, "demoTemplate", typeOf(ClassName), templateDemoModel);

            // Finally Send the Email
            var emailContent = new MailMessage(_emailAddress, "recipientEmail@emailAddress.com")
            {
                Subject = "Demo Email",
                IsBodyHtml = true,
                Body = messageBody
            };

            await _mailServer.SendMailAsync(emailContent);
        }
    }
}