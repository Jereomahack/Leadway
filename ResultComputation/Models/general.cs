using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace LightWay.Models
{
    public class general
    {

        public void SendMail(string Message, string Topic, string ReceivingMail)
        {
            //sending Email to Agents
            string FromMail = "funsho@osoftint.com";
            string SMTP = "sv5.trivaluehost.com";
            string MailPassword = "funsho12345";


            System.Net.Mail.MailMessage mailObj = new System.Net.Mail.MailMessage(
            FromMail, ReceivingMail, Topic, Message);

            mailObj.CC.Add("wole@osoftint.com");

            SmtpClient SMTPServer = new SmtpClient(SMTP);

            SMTPServer.Credentials = new System.Net.NetworkCredential(FromMail, MailPassword);
            SMTPServer.Send(mailObj);
        }
    }
}