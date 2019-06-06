using System;
using PgsKanban.BusinessLogic.Enums;

namespace PgsKanban.BusinessLogic.Options
{
    public class FacebookOptions
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string ProfileInfoUrl { get; set; }
        public string ClientLoginEndpoint { get; set; }
        public string EmailKey => "email";
        public string DataKey => "data";
        public string PictureUrlKey => "url";
        public string FirstNameKey => "first_name";
        public string LastNameKey => "last_name";
        public string PictureKey => "picture";
    }
}
