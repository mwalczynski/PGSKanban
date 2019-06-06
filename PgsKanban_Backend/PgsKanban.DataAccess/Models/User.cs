using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PgsKanban.BusinessLogic.Enums;
using PgsKanban.DataAccess.Interfaces;

namespace PgsKanban.DataAccess.Models
{
    public class User : IdentityUser, IDbEntity
    {
        [Required, MaxLength(256)]
        public string FirstName { get; set; }
        [Required, MaxLength(256)]
        public string LastName { get; set; }
        public virtual ICollection<UserBoard> Boards { get; set; }
        public DateTime? EmailConfirmationTokenExpirationTime { get; set; }
        public DateTime? PasswordResetTokenExpirationTime { get; set; }
        public string EmailConfirmationToken { get; set; }
        public string PasswordResetToken { get; set; }
        public bool IsProfileAnonymous { get; set; }
        public bool IsDeleted { get; set; }
        public ExternalLoginProvider RegisteredWith { get; set; }
        public string PictureSrc { get; set; }
    }
}
