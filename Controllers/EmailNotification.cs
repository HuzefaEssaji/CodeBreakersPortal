using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Source.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Controllers
{
    public class EmailNotification : Controller
    {
        public static void studentTask(AdminModel adminModel)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("The CodeBreakers", "thecodebreakers.rcoem@gmail.com"));
            mailMessage.To.Add(new MailboxAddress(adminModel.StudentEmail, adminModel.StudentEmail));
            mailMessage.Subject = "New Task Allotted";
            mailMessage.Body = new TextPart("plain")
            {
                Text = " Task : " + adminModel.UpdateTask + "\n\nCheck out the new task allotted to you at www.codebreakers.somee.com"
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.Authenticate("thecodebreakers.rcoem@gmail.com", "Thecodebreakers#rcoem");
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }

        public static void newUser(UserModel userModel)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("The CodeBreakers", "thecodebreakers.rcoem@gmail.com"));
            mailMessage.To.Add(new MailboxAddress(userModel.Email, userModel.Email));
            mailMessage.Subject = "Credentials";
            mailMessage.Body = new TextPart("plain")
            {
                Text = "Welcome, to the Team CodeBreakers." +
                "\n\n Here are your credentials" +
                "\n\n Username:  " + userModel.Email +
                "\n\n Password:   " + userModel.Password +
                "\n\n Role: " + userModel.Role
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.Authenticate("thecodebreakers.rcoem@gmail.com", "Thecodebreakers#rcoem");
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}
