namespace MedicalCenter.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public int? AppointmentId { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Appointment? Appointment { get; set; }
    }
}