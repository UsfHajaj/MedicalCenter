namespace MedicalCenter.Services.impelementation
{
    public class EmailTemplateService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailTemplateService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string GetConfirmationEmail(string userName,string confirmationLink)
        {
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmailTemplates", "ConfirmationEmail.html");

            var emailTemplate = File.ReadAllText(templatePath);

            var emailBody = emailTemplate.Replace("{UserName}", userName)
                .Replace("{ConfirmationLink}", confirmationLink);

            return emailBody;
        } 

        public string GetAppointmentConfirmationEamil(string patientName,string doctorName,string date)
        {
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmailTemplates", "ConfirmAppointment.html");

            var emailTemplate = File.ReadAllText(templatePath);
            var emailBody = emailTemplate.Replace("{PatientName}", patientName)
                .Replace("{DoctorName}", doctorName)
                .Replace("{Date}", date);
            return emailBody;
        }


    }
}
