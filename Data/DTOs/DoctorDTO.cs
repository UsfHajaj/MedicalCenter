namespace MedicalCenter.Data.DTOs
{
    public class DoctorDTO
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Email { get; set; }
        public string? ProfessionalStatement { get; set; }
        public DateTime? PracticingFrom { get; set; }
        public List<string>? Specializations { get; set; }
        public List<int>? QualificationIds { get; set; }
        public List<int>? SpecializationIds { get; set; }
        public List<int>? HospitalAffiliationIds { get; set; }
    }
}
