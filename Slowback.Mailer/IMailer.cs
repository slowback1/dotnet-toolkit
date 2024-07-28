namespace Slowback.Mailer;

public interface IMailer
{
    void Send(MailMessage message);
}