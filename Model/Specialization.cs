namespace MedicalCenter.Model
{
    public class Specialization
    {
        public int Id { get; set; }
        public string? SpecializationName { get; set; } = string.Empty;
        public string? SpecializationImage { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool? IsActive { get; set; } = true;
        public List<Service>? Services { get; set; } = new List<Service>();
        public List<DoctorSpecialization>? DoctorSpecializations { get; set; } = new();
    }
}