﻿namespace Slowback.Mailer;

public struct MailMessage
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}