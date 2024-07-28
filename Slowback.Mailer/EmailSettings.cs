namespace Slowback.Mailer;

public struct EmailSettings
{
    public string SmtpHost { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public string FromAddress { get; set; }
    public string FromName { get; set; }
}