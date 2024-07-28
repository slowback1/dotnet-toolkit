using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Slowback.Common;
using Slowback.Messaging;

namespace Slowback.Mailer;

public class MailService : MessageBusListener<MailMessage>
{
    private readonly IMailer _mailer;

    public MailService(IMailer mailer) : base(Messages.SendEmail)
    {
        _mailer = mailer;
    }

    public override Task OnMessage(MailMessage message)
    {
        TrySendMessage(message);

        return Task.CompletedTask;
    }

    private void TrySendMessage(MailMessage message, int timesCalled = 1)
    {
        try
        {
            _mailer.Send(message);
        }
        catch (SmtpException)
        {
            if (timesCalled < 3) TrySendMessage(message, timesCalled + 1);
            else MessageBus.Publish(Messages.LogMessage, $"Failed to send email to {message.To} after 3 attempts.");
        }
        catch (Exception e)
        {
            MessageBus.Publish(Messages.LogMessage, e.ToString());
        }
    }
}