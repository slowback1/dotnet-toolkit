﻿using System;
using System.Net;

namespace Slowback.Mailer;

public interface ISmtpClient : IDisposable
{
    ICredentialsByHost Credentials { get; set; }
    bool EnableSsl { get; set; }
    void Send(System.Net.Mail.MailMessage message);
}