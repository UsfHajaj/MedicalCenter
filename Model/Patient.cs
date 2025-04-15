using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedicalCenter.Model
{
    public class Patient:ApplicationUser
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Address { get; set; }
        [JsonIgnore]
        public ICollection<PatientReview> PatientReviews { get; set; } = new List<PatientReview>();
    }
}
