namespace MedicalCenter.Model
{
    public class DoctorQualification
    {
        public int Id { get; set; }
        public string? DoctorId { get; set; }
        public string? QualificationName { get; set; } = string.Empty;
        public string? InstituteName { get; set; } = string.Empty;
        public DateTime? ProcurementYear { get; set; }

        public Doctor? Doctor { get; set; } = null!;
    }
}