using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Common
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Stream attachment);
    }
}
