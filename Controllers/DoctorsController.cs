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


        
    }
}
