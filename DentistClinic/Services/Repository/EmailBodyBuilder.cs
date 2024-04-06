using DentistClinic.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;


namespace DentistClinic.Services.Repository
{
    public class EmailBodyBuilder : IEmailBodyBuilder
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmailBodyBuilder(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public string GetEmailBody(string template , Dictionary<string , string> placeholders)
        {
            var templatePath = Path.Combine(webHostEnvironment.WebRootPath, $"templates/{template}.html");
            StreamReader streamReader = new StreamReader(templatePath);

            var htmlBody = streamReader.ReadToEnd();
            streamReader.Close();

            foreach (var placeholder in placeholders)
            {
                htmlBody = htmlBody.Replace($"[{placeholder.Key}]", placeholder.Value);
            }

            return htmlBody;
        }
    }
}
