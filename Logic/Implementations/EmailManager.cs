using System.Net.Mail;
using Logic.Interfaces;

namespace Logic.Implementations;

public class EmailManager: IEmailManager
{
    public void SendMessageAsync(string email, string subject, string message)
    {
        return;
        // var source = new MailAddress("mukanovarman777@gmail.com", "SchooLearn");
        // var dest = new MailAddress(email, "data");
        // var mailMessage = new MailMessage(source, dest);
        // mailMessage.Body = message;
        // mailMessage.Subject = subject;
        //
        // var mySmtpClient = new SmtpClient("my.smtp.exampleserver.net");
        
        // schoolearn-robot@mail.ru
        // mukanovarman777@gmail.com
        var mySmtpClient = new SmtpClient("smtp.mail.com");
        // var mySmtpClient = new SmtpClient("smtp.mail.com", 587);
        // set smtp-client with basicAuthentication
        mySmtpClient.UseDefaultCredentials = false;
        mySmtpClient.EnableSsl = true;
        var basicAuthenticationInfo = new
            System.Net.NetworkCredential("schoolearn-robot@mail.ru", "WCYxajaBsY7jgsfWvqmi");
        mySmtpClient.Credentials = basicAuthenticationInfo;

        // add from,to mail addresses
        MailAddress from = new MailAddress("schoolearn-robot@mail.ru", "SchooLearn");
        MailAddress to = new MailAddress("schoolearn-robot@mail.ru", "TestTo");
        MailMessage myMail = new MailMessage(from, to);

        // add ReplyTo
        MailAddress replyTo = new MailAddress("schoolearn-robot@mail.ru");
        myMail.ReplyToList.Add(replyTo);

        // set subject and encoding
        myMail.Subject = "Test message";
        myMail.SubjectEncoding = System.Text.Encoding.UTF8;

        // set body-message and encoding
        myMail.Body = "<b>Test Mail</b><br>using <b>HTML</b>.";
        myMail.BodyEncoding = System.Text.Encoding.UTF8;
        // text or html
        myMail.IsBodyHtml = true;
        mySmtpClient.Send(myMail);
    }

    private bool CheckEmail(string email)
    {
        return false;
    }
}