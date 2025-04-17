using MedicalCenter.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpecializationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations()
        {
            return await _context.Specializations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(int id)
        {
            var result = await _context.Specializations.FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialization(int id, Specialization specialization)
        {
            if (id != specialization.Id)
                return NotFound();
            _context.Entry(specialization).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecializationsExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentStatus>> PostSpecialization(Specialization specialization)
        {
            await _context.AddAsync(specialization);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSpecialization", new { id = specialization.Id }, specialization);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            var specialization = await _context.Specializations
                //.Include(s => s.Services)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specialization == null)
                return NotFound();
            _context.Specializations.Remove(specialization);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool SpecializationsExists(int id)
        {
            return _context.Specializations.Any(e => e.Id == id);
        }
    }
}
