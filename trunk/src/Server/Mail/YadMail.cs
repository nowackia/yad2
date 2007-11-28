using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Mail {
    public class YadMail {
        private const string MessageSubject = "Yet Another Dune II Password Reminder";
        private const string MessageTextFormat  = "Hello {0},/r/nYour password is: {1}";
        private const string SMTPServer = "poczta.o2.pl";
        public static bool SendRemindMail(string name, string email, string password){
            System.Web.Mail.MailMessage message = new System.Web.Mail.MailMessage();

            message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", 1);
            message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "yadmail");
            message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "dostaniemy5");

            message.From = "yadmail@o2.pl";
            message.To = email;
            message.Subject = MessageSubject;
            message.Body = string.Format(MessageTextFormat, name, email);

            System.Web.Mail.SmtpMail.SmtpServer = SMTPServer;
            try {
                System.Web.Mail.SmtpMail.Send(message);
            }
            catch (Exception) {
                return false;
            }
            return true;
        }
    }
}
