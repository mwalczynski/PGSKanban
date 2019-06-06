using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using System.IO;
using PgsKanban.DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.BusinessLogic.Options;

namespace PgsKanban.BusinessLogic.Implementation
{
    public class MessageSender : IMessageSender
    {
        private readonly MessageSenderOptions _configuration;
        private readonly IHostingEnvironment _env;
        private readonly string _baseUrl;
        private readonly IUserService _userService;

        public MessageSender(IOptions<MessageSenderOptions> senderOptions, IHostingEnvironment environment, IConfigurationRoot configuration, IUserService userService)
        {
            _configuration = senderOptions.Value;
            _env = environment;
            _baseUrl = configuration["ApplicationUrl:localhost"];
            _userService = userService;
        }

        public string GetActivationTemplateId()
        {
            return _configuration.AccountActivationTemplateName;
        }

        public string GetResetPasswordTemplateId()
        {
            return _configuration.ResetPasswordTemplateName;
        }

        public string GetChangePasswordTemplateId()
        {
            return _configuration.ChangePasswordTemplateName;
        }

        public void SendEmail(string subject, string templateId, User user, string link)
        {
            Execute(subject, templateId, user, link);
        }

        public void SendEmail(string subject, string templateId, User user, string userBrowser, string userBrowserVersion)
        {
            Execute(subject, templateId, user, userBrowser, userBrowserVersion);
        }

        private void Execute(string subject, string templateName, User user, string link)
        {
            var template = PrepareTemplate(templateName, user, link);

            var client = new SmtpClient
            {
                Port = _configuration.ServerPort,
                Host = _configuration.ServerHost
            };
            var msg = new MailMessage
            {
                From = new MailAddress("pgskanban@gmail.com", "PGS Kanban"),
                Subject = subject,
                IsBodyHtml = true,
                Body = template,
            };
            msg.To.Add(new MailAddress(user.Email));
            client.SendMailAsync(msg);
        }

        private void Execute(string subject, string templateName, User user, string userBrowser, string userBrowserVersion)
        {
            var template = PrepareTemplate(templateName, userBrowser, userBrowserVersion);

            var client = new SmtpClient
            {
                Port = _configuration.ServerPort,
                Host = _configuration.ServerHost
            };

            var msg = new MailMessage
            {
                From = new MailAddress("pgskanban@gmail.com", "PGS Kanban"),
                Subject = subject,
                IsBodyHtml = true,
                Body = template,
            };
            msg.To.Add(new MailAddress(user.Email));
            client.SendMailAsync(msg);
        }

        private string PrepareTemplate(string templateName, User user, string link)
        {
            var path = Path.Combine(_env.ContentRootPath, $"{templateName}.html");
            var template = File.ReadAllText(path);
            var substitutions = CreateSubstitutions(user, link);
            foreach (var substitution in substitutions)
            {
                template = template.Replace(substitution.Key, substitution.Value);
            }
            return template;
        }

        private string PrepareTemplate(string templateName, string userBrowser, string userBrowserVersion)
        {
            var path = Path.Combine(_env.ContentRootPath, $"{templateName}.html");
            var template = File.ReadAllText(path);
            var substitutions = GatherInformation(userBrowser, userBrowserVersion);
            foreach (var substitution in substitutions)
            {
                template = template.Replace(substitution.Key, substitution.Value);
            }
            return template;
        }

        private Dictionary<string, string> CreateSubstitutions(User user, string link)
        {
            var substitutions = new Dictionary<string, string>
            {
                { "[%first_name%]", user.FirstName },
                { "[%last_name%]", user.LastName },
                { "[Weblink]", link },
                {"[BaseUrl]", _baseUrl }
            };
            return substitutions;
        }

        private Dictionary<string, string> GatherInformation(string userBrowser, string userBrowserVersion)
        {
            var localization = _userService.GetUserLocalization().Result;
            var substitution = new Dictionary<string, string>
            {
                { "[%user_ip%]", _userService.GetUserIp() },
                { "[%user_localization%]", localization },
                { "[%user_browser%]", userBrowser },
                { "[%user_browserVersion%]", userBrowserVersion },
                { "[BaseUrl]", _baseUrl }
            };
            return substitution;
        }
    }
}
