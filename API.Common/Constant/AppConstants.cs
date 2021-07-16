namespace API.Common
{
    public class SettingConfig
    { 
        public AppConfig App { get; set; }
        public SettingConfig()
        { 
            App = new AppConfig();
        } 
    }
    public class AppConfig
    { 
        public string WebAPISecurityKey { get; set; }
        public bool ShowWebAPISecurityKeyInDoc { get; set; }
    }
}
