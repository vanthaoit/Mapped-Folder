namespace LogixHealth.EnterpriseLibrary.Email.Gateway
{
    public class EmailClient : IEmailClient
    {
        private readonly string _emailServer;
        private readonly int _emailServerPort;

        public EmailClient(string smtp, string port)
        {
            _emailServer = smtp;
            _emailServerPort = System.Convert.ToInt32(port);
        }

        public void SendEmail(string from, string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(from))
            {
                throw new System.ArgumentNullException(nameof(from));
            }

            if (string.IsNullOrWhiteSpace(to))
            {
                throw new System.ArgumentNullException(nameof(to));
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new System.ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new System.ArgumentNullException(nameof(body));
            }

            using (System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(_emailServer, _emailServerPort))
            {
                smtpClient.Send(new System.Net.Mail.MailMessage(from, to, subject, body));
            }
        }

        public void SendEmail(string from, string to, string subject, string body, string referenceNumber)
        {
            if (string.IsNullOrWhiteSpace(from))
            {
                throw new System.ArgumentNullException(nameof(from));
            }

            if (string.IsNullOrWhiteSpace(to))
            {
                throw new System.ArgumentNullException(nameof(to));
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new System.ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new System.ArgumentNullException(nameof(body));
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new System.ArgumentNullException(nameof(referenceNumber));
            }

            using (System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(_emailServer, _emailServerPort))
            {
                smtpClient.Send(new System.Net.Mail.MailMessage(from, to, subject + " Reference# " + referenceNumber , body));
            }
        }
    }
}
