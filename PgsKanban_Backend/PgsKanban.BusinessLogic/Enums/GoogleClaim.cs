using System.ComponentModel;

namespace PgsKanban.BusinessLogic.Enums
{
    public enum GoogleClaim
    {
        [Description("email")]
        Email,
        [Description("given_name")]
        FirstName,
        [Description("family_name")]
        LastName,
        [Description("picture")]
        Picture
    }
}
