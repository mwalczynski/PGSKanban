using System.ComponentModel;

namespace PgsKanban.BusinessLogic.Enums
{
    public enum OpenIdClaim
    {
        [Description("email")]
        Email,
        [Description("first_name")]
        FirstName,
        [Description("last_name")]
        LastName,
        [Description("thumbnail")]
        Thumbnail
    }
}
