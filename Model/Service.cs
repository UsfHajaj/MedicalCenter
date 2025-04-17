namespace MedicalCenter.Model
{
    public class Service
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int SpecializationId { get; set; }

        // Navigation property
        public Specialization? Specialization { get; set; }
    }
}