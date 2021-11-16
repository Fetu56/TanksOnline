using System.Net;
using System.Net.Mail;

namespace ClientGame.Log
{
    public class EmailSend
    {
        public static void Send(string email, string code, string log)
        {
            MailAddress from = new MailAddress("maksim7892935@gmail.com", "Tanks online");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Tanks online";
            m.Body = $"<h2>Здравствуйте. Вот ваш код подтверждения аккаунта с логином {log}: {code}</h2>";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("maksim7892935@gmail.com", "pass");
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}
