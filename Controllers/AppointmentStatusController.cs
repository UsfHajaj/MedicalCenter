using MedicalCenter.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedicalCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentStatusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentStatus>>> GetAppoinetmentStatuses()
        {
            return await _context.AppointmentStatus.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentStatus>> GetAppoinetmentStatuse(int id)
        {
            var result =await _context.AppointmentStatus.FirstOrDefaultAsync(m=>m.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointmentStatus(int id , AppointmentStatus appointmentStatus)
        {
            if (id != appointmentStatus.Id)
                return NotFound();
            _context.Entry(appointmentStatus).State = EntityState.Modified;
            try
            {
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentStatusExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentStatus>> PostAppointmentStatus(AppointmentStatus appointmentStatus)
        {
            await _context.AddAsync(appointmentStatus);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAppoinetmentStatuse", new { id = appointmentStatus.Id }, appointmentStatus);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentStatus(int id)
        {
            var appointmentStatus = await _context.AppointmentStatus.FirstOrDefaultAsync(m=>m.Id==id);
            if (appointmentStatus == null)
                return NotFound();
            _context.AppointmentStatus.Remove(appointmentStatus);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool AppointmentStatusExists(int id)
        {
            return _context.AppointmentStatus.Any(e => e.Id == id);
        }
    }
}
