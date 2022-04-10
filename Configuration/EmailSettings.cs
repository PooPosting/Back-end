namespace PicturesAPI.Configuration;

public class EmailSettings
{
    public string SendLogsToId { get; set; }
    public string SendLogsToName { get; set; }
    public string EmailId { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool UseSsl { get; set; }
}