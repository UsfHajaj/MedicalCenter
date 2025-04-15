using MedicalCenter.Data.DTOs;
using MedicalCenter.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedicalCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _context.Patients
                .Include(p => p.PatientReviews)
                .Select(m => new
                {
                    PatientId = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    Image = m.Image,
                    Reviews = m.PatientReviews

                }).ToListAsync();

            return Ok(patients);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetPatientById(string id)
        {
            var patient = await _context.Patients
                .Where(m => m.Id == id)
                .Select(p => new PatientDTO
                {
                    PatientId = p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    Image = p.Image,

                }).FirstOrDefaultAsync();

            if (patient == null) return NotFound();

            return Ok(patient);
        }

        [HttpGet("{id}/appointments")]
        public async Task<IActionResult> GetPatientAppointments(string id)
        {
            var appointments = await _context.Appointments.Where(m => m.PatientId == id).ToListAsync();

            return Ok(appointments);
        }


        [HttpGet("{id}/appointments/date-range")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByDate(string id, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var appointment = await _context.Appointments
                .Where(m => m.PatientId == id && m.AppointmentTakenDate >= start && m.AppointmentTakenDate <= end)
                .ToListAsync();
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }


        [HttpPut("{id}/reviews/{reviewId}")]
        public async Task<IActionResult> UpdateReviews(string id, int reviewId, [FromBody] PatientReview review)
        {
            if (review.PatientId != id || review.Id != reviewId)
            {
                return BadRequest("Patient ID or Review ID mismatch.");
            }
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}/appointments/{appId}")]
        public async Task<IActionResult> DeleteAppointment(string id, int appId)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(m => m.Id == appId && m.PatientId == id);
            if (appointment == null) return NotFound();
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(string id,[FromBody] PatientDTO patientFromRequest)
        {
            var patientFromDb = await _context.Patients.FirstOrDefaultAsync(m => m.Id == id);

            if (patientFromDb == null) return NotFound();

            patientFromDb.Name = patientFromRequest.Name;
            patientFromDb.Email = patientFromRequest.Email;
            patientFromDb.Image = patientFromRequest.Image;

            _context.Patients.Update(patientFromDb);

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(string id)
        {
            var patientFromDb = await _context.Patients.FirstOrDefaultAsync(m => m.Id == id);

            if (patientFromDb == null) return NotFound();

            _context.Patients.Remove(patientFromDb);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
