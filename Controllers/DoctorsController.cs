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
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctor = await _context.Doctors.Include(m => m.DoctorSpecializations)!
                .ThenInclude(m => m.Specialization)
                .ToListAsync();
            var result = doctor.Select(d => new DoctorDTO
            {
                Id = d.Id,
                Name = d.Name,
                Image = d.Image,
                Email=d.Email,
                ProfessionalStatement = d.ProfessionalStatement,
                PracticingFrom = d.PracticingFrom,
                Specializations = d.DoctorSpecializations!.Select(d=>d.Specialization!.SpecializationName).ToList()!,
                QualificationIds = d.Qualifications!.Select(d => d.Id).ToList(),
                HospitalAffiliationIds = d.HospitalAffiliations!.Select(d => d.Id).ToList()
            }).ToList();

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(string id)
        {
            var doctor = await _context.Doctors.Select(d => new
            {
                Id=d.Id,
                Name=d.Name,
                Email=d.Email,
                Image=d.Image,
                ProfessionalStatement = d.ProfessionalStatement,
                PracticingFrom = d.PracticingFrom,
                Specializations = d.DoctorSpecializations!.Select(d => d.Specialization!.SpecializationName).ToList()!,
                Qualifications=d.Qualifications!.Select(d=>d.QualificationName),
                HospitalAffiliations=d.HospitalAffiliations!.Select(d=>d.HospitalName)
            }).FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null) return NotFound();


            return Ok(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> PostDoctor(DoctorDTO doctorFromRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var doctorFromDb = new Doctor
            {
                Id = doctorFromRequest.Id,
                Name = doctorFromRequest.Name,
                Image = doctorFromRequest.Image,
                ProfessionalStatement = doctorFromRequest.ProfessionalStatement,
                PracticingFrom = doctorFromRequest.PracticingFrom,
            };

            if (doctorFromRequest.SpecializationIds != null)
            {
                doctorFromDb.DoctorSpecializations = doctorFromRequest.SpecializationIds.Select(id => new DoctorSpecialization
                {
                    SpecializationId = id
                }).ToList()!;
            }
            if (doctorFromRequest.QualificationIds != null)
            {
                doctorFromDb.Qualifications = doctorFromRequest.QualificationIds.Select(id => new DoctorQualification
                {
                    Id = id
                }).ToList()!;
            }
            if (doctorFromRequest.HospitalAffiliationIds != null)
            {
                doctorFromDb.HospitalAffiliations = doctorFromRequest.HospitalAffiliationIds.Select(id => new HospitalAffiliation
                {
                    Id = id
                }).ToList()!;
            }

            await _context.Doctors.AddAsync(doctorFromDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctorById), new { id = doctorFromDb.Id }, doctorFromDb);
        }

        [HttpGet("{Id}/bookings")]
        public async Task<IActionResult> GetBookings(string id)
        {
            var booking = await _context.Appointments
                .Include(m => m.Patient)
                .Where(m => m.DoctorId == id && m.AppointmentStatus!.Status == AppointmentStatusEnum.Active)
                .ToListAsync();

            return Ok(booking);
        }

        [HttpGet("{id}/bookings/status/{status}")]
        public async Task<IActionResult> GetBookingsWithStatus(string id,AppointmentStatusEnum status)
        {
            var booking =  await _context.Appointments
                .Include(m => m.AppointmentStatus)
                .Where(m => m.DoctorId == id && m.AppointmentStatus!.Status == status)
                .ToListAsync();
            return Ok(booking);
        }

        [HttpGet("{id}/bookings/status/today")]
        public async Task<IActionResult> GetTodaysBookings(string id)
        {
            var today = DateTime.Today;
            var booking = await _context.Appointments
                .Include(m => m.Patient)
                .Where(m => m.DoctorId == id && m.AppointmentTakenDate== today&& m.AppointmentStatus!.Status == AppointmentStatusEnum.Active)
                .ToListAsync();
            return Ok(booking);
        }
        [HttpGet("{id}/bookings/status/upcoming")]
        public async Task<IActionResult> GetBookingsUpComing(string id)
        {
            var today = DateTime.Today;
            var booking = await _context.Appointments
                .Include(m => m.Patient)
                .Where(m => m.DoctorId == id && m.AppointmentTakenDate >= today && m.AppointmentStatus!.Status == AppointmentStatusEnum.Active)
                .ToListAsync();
            return Ok(booking);
        }
        [HttpGet("{id}/bookings/status/last30day")]
        public async Task<IActionResult> GetBookingsLast30Day(string id)
        {
            var today = DateTime.Today;
            var last30Days = today.AddDays(-30);
            var result = await _context.Appointments
                .Include(m => m.Patient)
                .Where(m => m.DoctorId == id &&
                m.AppointmentTakenDate <= today
                && m.AppointmentTakenDate >= last30Days
                && m.AppointmentStatus!.Status == AppointmentStatusEnum.Active)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetReviews(string id)
        {
            var today = DateTime.Today;
            var result = await _context.PatientReviews
                .Include(m => m.Patient)
                .Where(m => m.DoctorId == id )
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}/Rating")]
        public async Task<IActionResult> GetRating(string id)
        {
            var today = DateTime.Today;
            var Rating = await _context.PatientReviews
                .Where(m => m.DoctorId == id)
                .AverageAsync(m => m.OverallRating);
            return Ok(Rating);
        }

        [HttpGet("{id}/qualifications")]
        public async Task<IActionResult> GetQualifications(string id)
        {
           
            var result = await _context.DoctorQualifications
                .Where(m => m.DoctorId == id)
                .ToListAsync();
            return Ok(result);
        }
        [HttpGet("{id}/specializations")]
        public async Task<IActionResult> Getspecializations(string id)
        {

            var result = await _context.DoctorSpecialization
                .Include(m => m.Specialization)
                .Where(m => m.DoctorId == id)
                .ToListAsync();
            return Ok(result);
        }

        [HttpPut("Id")]
        public async Task<IActionResult> PutDoctor (string id, Doctor doctorFromRequest)
        {
            if (id != doctorFromRequest.Id)
                return BadRequest();


            
            _context.Entry(doctorFromRequest).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpPut("bookings/{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] Appointment updatedBooking)
        {
            var booking = await _context.Appointments.FindAsync(id);

            if (booking == null)
                return NotFound();

            booking.AppointmentStatus = updatedBooking.AppointmentStatus;
            booking.AppointmentTakenDate = updatedBooking.AppointmentTakenDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound();
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}/appointments/{appointmentid}")]
        public async Task<IActionResult> DeleteAppointment(string id ,int appointmentid)
        {
            var appointment= await _context.Appointments
                .Include(m=>m.AppointmentStatus)
                .Where(m=>m.DoctorId==id&&m.Id==appointmentid)
                .FirstOrDefaultAsync();
            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });
            appointment.AppointmentStatusId = (int)AppointmentStatusEnum.Canceled;
            await _context.SaveChangesAsync();
            return NoContent();
        }




        private bool DoctorExists(string id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
