

namespace PresentationLayer.Utilities
{
    public class Email
    {
		public string Subject { get; set; }
		public string Body { get; set; }
		public string Recipient { get; set; }
	}
	public static class MailSettings
	{
       public static void SendMail(Email email)
		{
			// create smtp client
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;

			//create network credentials
			client.Credentials = new NetworkCredential("monaayman2911@gmail.com", "cazbwgqtwtovcjoo");
			client.Send("monaayman2911@gmail.com", email.Recipient, email.Subject, email.Body);
		}
    }
}
