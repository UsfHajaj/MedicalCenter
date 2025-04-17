using MedicalCenter.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalCentersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicalCentersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalCenters>>> GetMedicalCenter()
        {
            return await _context.MedicalCenter.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalCenters>> GetMedicalCenter(int id)
        {
            var result = await _context.MedicalCenter.FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalCenter(int id, MedicalCenters medicalCenters)
        {
            if (id != medicalCenters.Id)
                return NotFound();
            _context.Entry(medicalCenters).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalCenterExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<MedicalCenters>> PostMedicalCenter(MedicalCenters medicalCenters)
        {
            await _context.AddAsync(medicalCenters);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetMedicalCenter", new { id = medicalCenters.Id }, medicalCenters);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalCenter(int id)
        {
            var result = await _context.MedicalCenter.FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
                return NotFound();
            _context.MedicalCenter.Remove(result);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool MedicalCenterExists(int id)
        {
            return _context.MedicalCenter.Any(e => e.Id == id);
        }
    }
}
