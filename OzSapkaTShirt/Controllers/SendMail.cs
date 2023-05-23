using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace EasyCashIdentityProject.EntityLayer.Concrete
{
    internal class SendMail
    {
        private readonly string _appUser;
        private readonly string _mailFrom;
        private readonly string _mailPassword;
        private readonly string _mailTo;
        private readonly string _Admin_UserName;
        private readonly string _User_UserName;
        private readonly string _message;
        private readonly string _subject;
        /// <summary>
        /// Before we use this download package "NETCore.MailKit - by Lvcc"
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="mailFrom"></param>
        /// <param name="mailPassword"></param>
        /// <param name="Admin_UserName"></param>
        /// <param name="User_UserName"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public SendMail(string mailTo, string mailFrom, string mailPassword, string? Admin_UserName, string? User_UserName, string? subject, string? message)
        {
            _mailTo = mailTo;
            _mailFrom = mailFrom;
            _mailPassword = mailPassword;
            _Admin_UserName = Admin_UserName;
            _User_UserName = User_UserName;
            _message = message;
            _subject = subject;
            var mime = CreateMail(_Admin_UserName, _User_UserName, _subject, _message);
            SendMailTo(mime);
        }

        private void SendMailTo(MimeMessage mime)
        {
            SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect("smtP.gmail.com", 587, false);
            client.Authenticate(_mailFrom, _mailPassword);
            client.Send(mime);
            client.Disconnect(true);
        }
        private MimeMessage CreateMail(string? _Admin_UserName = "Admin", string? _User_UserName = "User", string? subject = "Send Message", string? message = "Mesaj Gönderildi")
        {
            BodyBuilder bodyBuilder = new BodyBuilder();
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAdressFrom = new MailboxAddress(_Admin_UserName, _mailFrom);
            MailboxAddress mailboxAdressTo = new MailboxAddress(_User_UserName, _mailTo);
            mimeMessage.From.Add(mailboxAdressFrom);
            mimeMessage.To.Add(mailboxAdressTo);
            bodyBuilder.TextBody = message;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = subject;
            return mimeMessage;
        }
    }
}
