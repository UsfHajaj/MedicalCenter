using MedicalCenter.Data.DTOs;
using MedicalCenter.Model;
using MedicalCenter.Services;
using MedicalCenter.Services.impelementation;
using MedicalCenter.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailTemplateService _emailTemplateService;
        private readonly IEmailService _emailService;

        public AppointmentsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            EmailTemplateService emailTemplateService,IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailTemplateService = emailTemplateService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            var appointments = await _context.Appointments
                .Include(m=>m.Patient)
                .ToListAsync();
            return appointments;
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            if (!string.IsNullOrEmpty(appointment.DoctorName))
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(m => appointment.DoctorName == m.Name);
                if (doctor == null)
                {
                    return BadRequest("Invalid DoctorName");
                }
                appointment.DoctorId = doctor.Id;
            }

            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user= await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Invalid User");
            }
            appointment.PatientId = user.Id;
            appointment.MedicalCenterId = 2;
            appointment.AppointmentStatusId = (int)AppointmentStatusEnum.Active+(int)1;
            appointment.Amount = 30;
            appointment.PaymentStatus = "Pending";
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            var emailTemplateService = HttpContext.RequestServices.GetRequiredService<EmailTemplateService>();

            var emailBody = _emailTemplateService.
                GetAppointmentConfirmationEamil(user.UserName!, appointment.DoctorName
                , appointment.AppointmentTakenDate.ToString()!);
            var massageIbj =new Message(new[] { user.Email! }, "Appointment Confirmation", emailBody);

            _emailService.SendEmail(massageIbj);


            return CreatedAtAction("GetAppointmentById", new { id = appointment.Id }, appointment);
        }

        [HttpGet("GetAllAppointments")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                .Include(m => m.Doctor)
                .ThenInclude(m => m.DoctorSpecializations)!
                .ThenInclude(m => m.Specialization)
                .Include(m => m.Patient)
                .ToListAsync();

            var result = appointments.Select(m => new
            {
                AppointmentId =m.Id,
                AppointmentDate=m.AppointmentTakenDate,
                Doctor=new DoctorDTO
                {
                    Name = m.DoctorName,
                    Specializations = m.Doctor?.DoctorSpecializations
                        .Select(ds => ds.Specialization?.SpecializationName)
                        .ToList()??new List<string>()
                },
                Patient = new PatientDTO
                {
                    PatientId=m.PatientId!.ToString(),
                    Name = m.Patient?.UserName,
                    Email = m.Patient?.Email

                }
            }).ToList();

            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
        {
            var result= await _context.Appointments.FirstOrDefaultAsync(m=>m.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, UpdateAppointmentDTO appointmentDTO)
        {
            if (id != appointmentDTO.Id)
            {
                return BadRequest();
            }
            var appoiFromRes= await _context.Appointments.FirstOrDefaultAsync(m => m.Id == id);
            if (appoiFromRes == null)
            {
                return NotFound();
            }
            appoiFromRes.AppointmentTakenDate = appointmentDTO.AppointmentTakenDate;
            _context.Entry(appoiFromRes).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "An error occurred while updating the appointment.");
            }

            return NoContent();

        }

        [HttpGet("total-earnings")]
        public async Task<IActionResult> GetTotalErning()
        {
            var result = await _context.Appointments
                .SumAsync(m => m.Amount);
            return Ok(new { TotalEarnings = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(m=>m.Id==id);
            if (appointment == null)
            {
                return NotFound();
            }
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(string patientId)
        {
            var appointments = await _context.Appointments
                .Where(m => m.PatientId == patientId)
                .ToListAsync();
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound();
            }
            return Ok(appointments);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetAppointmentsByDate(DateTime date)
        {
            var appointments= await _context.Appointments
                .Where(m=>m.AppointmentTakenDate.Value.Date == date.Date)
                .ToListAsync();
            return Ok(appointments);
        }
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetAppointmentByStatus(AppointmentStatusEnum status)
        {
            var result = await _context.Appointments
                .Where(m => m.AppointmentStatus.Status == status)
                .ToListAsync();
            return Ok(result);
        }
        [HttpGet("today")]
        public async Task<IActionResult> GetAppointmentToday()
        {
            var result = await _context.Appointments
                .Where(m => m.AppointmentTakenDate!.Value.Date == DateTime.Today)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingAppointment()
        {
            var today = DateTime.Today;
            var result = await _context.Appointments
                .Where(m => m.ProbableStartTime > today)
                .OrderBy(m=>m.ProbableStartTime)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("patient/{patientId}/status/{status}")]
        public async Task<IActionResult> GetAppointmentByPatientIdAndStatus(string patientId, AppointmentStatusEnum status)
        {
            var result= await _context.Appointments
                .Where(m=>m.PatientId== patientId && m.AppointmentStatus!.Status == status)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("patient/{patientId}/history")]
        public async Task<IActionResult> GetPatientHistory(string patientId)
        {
            var result = await _context.Appointments
                .Where(m=>m.PatientId == patientId)
                .OrderByDescending(m=>m.ProbableStartTime)
                .ToListAsync();
            return Ok(result);
        }

        private bool AppointmentExists(int id)
        {
           return _context.Appointments.Any(m => m.Id == id);  
        }
    }
}
