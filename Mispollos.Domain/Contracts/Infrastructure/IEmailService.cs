using System;
using System.Collections.Generic;
using System.Text;

namespace Mispollos.Domain.Contracts.Infrastructure
{
    public interface IEmailService
    {
        void Send(string emailDestino, Guid token, String template, String subject);
    }
}