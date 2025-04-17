namespace MedicalCenter.Model
{
    public class MedicalCenterDoctorAvailability
    {
        public int Id { get; set; }
        public int? MedicalCenterId { get; set; }
        public string? DayOfWeek { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsAvailable { get; set; }
        public string? ReasonOfUnavailability { get; set; }

        public MedicalCenters? MedicalCenter { get; set; } = null!;
    }
}