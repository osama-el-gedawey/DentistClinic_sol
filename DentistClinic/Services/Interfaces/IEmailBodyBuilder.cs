namespace DentistClinic.Services.Interfaces
{
    public interface IEmailBodyBuilder
    {
        public string GetEmailBody(string template, Dictionary<string, string> placeholders);
    }
}
