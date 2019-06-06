using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface IConfirmAccountService
    {
        Task<string> SendEmailConfirmationToken(string email);
        Task<ConfirmationEmailResponseDto> ConfirmEmail(ConfirmEmailDto confirmEmailDto);
        Task<string> SendEmailConfirmationEmailByToken(string token);
    }
}
