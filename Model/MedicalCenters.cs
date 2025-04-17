namespace MedicalCenter.Model
{
    public class MedicalCenters
    {
        public int Id { get; set; }
        public int? HospitalAffiliationId { get; set; }
        public int? TimeSlotPerClientInMin { get; set; }
        public decimal? FirstConsultationFee { get; set; }
        public decimal? FollowupConsultationFee { get; set; }
        public string? StreetAddress { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? Zip { get; set; } = string.Empty;

        public ICollection<Doctor>? Doctors { get; set; } = new List<Doctor>();
        public HospitalAffiliation? HospitalAffiliation { get; set; }
        public ICollection<MedicalCenterDoctorAvailability>? MedicalCenterDoctorAvailabilities { get; set; } = new List<MedicalCenterDoctorAvailability>();
    }
}
