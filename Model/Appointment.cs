namespace MedicalCenter.Model
{
    public class Appointment
    {
        public int Id { get; set; }
        public string? PatientId { get; set; }
        public string? DoctorId { get; set; }

        public int? MedicalCenterId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? DoctorName { get; set; }
        public DateTime? ProbableStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public int? AppointmentStatusId { get; set; }
        public DateTime? AppointmentTakenDate { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentStatus { get; set; }
        public Payment? Payment { get; set; }
        public Doctor? Doctor { get; set; } = null!;
        public Patient? Patient { get; set; } = null!;
        public MedicalCenter? MedicalCenter { get; set; } = null!;
        public AppointmentStatus? AppointmentStatus { get; set; } = null!;
    }
}
