namespace CarCare_Companion.Core.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
