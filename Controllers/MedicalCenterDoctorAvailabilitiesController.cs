using MedicalCenter.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalCenterDoctorAvailabilitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicalCenterDoctorAvailabilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicalCenterDoctorAvailabilities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalCenterDoctorAvailability>>> GetMedicalCenterDoctorAvailability()
        {
            return await _context.MedicalCenterDoctorAvailability.ToListAsync();
        }

        // GET: api/MedicalCenterDoctorAvailabilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalCenterDoctorAvailability>> GetMedicalCenterDoctorAvailability(int id)
        {
            var medicalCenterDoctorAvailability = await _context.MedicalCenterDoctorAvailability.FindAsync(id);

            if (medicalCenterDoctorAvailability == null)
            {
                return NotFound();
            }

            return medicalCenterDoctorAvailability;
        }

        // PUT: api/MedicalCenterDoctorAvailabilities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalCenterDoctorAvailability(int id, MedicalCenterDoctorAvailability medicalCenterDoctorAvailability)
        {
            if (id != medicalCenterDoctorAvailability.Id)
            {
                return BadRequest();
            }

            _context.Entry(medicalCenterDoctorAvailability).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalCenterDoctorAvailabilityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MedicalCenterDoctorAvailabilities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MedicalCenterDoctorAvailability>> PostMedicalCenterDoctorAvailability(MedicalCenterDoctorAvailability medicalCenterDoctorAvailability)
        {
            _context.MedicalCenterDoctorAvailability.Add(medicalCenterDoctorAvailability);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicalCenterDoctorAvailability", new { id = medicalCenterDoctorAvailability.Id }, medicalCenterDoctorAvailability);
        }

        // DELETE: api/MedicalCenterDoctorAvailabilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalCenterDoctorAvailability(int id)
        {
            var medicalCenterDoctorAvailability = await _context.MedicalCenterDoctorAvailability.FindAsync(id);
            if (medicalCenterDoctorAvailability == null)
            {
                return NotFound();
            }

            _context.MedicalCenterDoctorAvailability.Remove(medicalCenterDoctorAvailability);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalCenterDoctorAvailabilityExists(int id)
        {
            return _context.MedicalCenterDoctorAvailability.Any(e => e.Id == id);
        }
    }
}
