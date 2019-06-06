using System.Threading.Tasks;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface IMessageSender
    {
        void SendEmail(string subject, string templateId, User user, string link);
        void SendEmail(string subject, string templateId, User user, string userBrowser, string userBrowserVersion);
        string GetResetPasswordTemplateId();
        string GetActivationTemplateId();
        string GetChangePasswordTemplateId();
    }
}
