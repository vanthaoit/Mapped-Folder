namespace LogixHealth.EnterpriseLibrary.Email.Gateway
{
    public interface IEmailClient
    {
        void SendEmail(string from, string to, string subject, string body);

        void SendEmail(string from, string to, string subject, string body, string referenceNumber);
    }
}
