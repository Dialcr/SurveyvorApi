namespace BePrácticasLaborales.Settings;

/*
public record MailSettings(
    string From,
    string Name,
    string smtp,
    int Port,
    string Password
);
*/
public class MailSettings
{
    public string From { get; set; }
    public string Name { get; set; }
    public string Smtp { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
    
    public string UrlWEB { get; set; }
    
}
