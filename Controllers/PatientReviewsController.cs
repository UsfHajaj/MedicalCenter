using MedicalCenter.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedicalCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetPatientReviews()
        {
            var reviews = _context.PatientReviews.ToList();
            return Ok(reviews);
        }
        [HttpGet("unique-patients")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetUniquePatients()
        {
            var uniquePatients = await _context.PatientReviews
                 .Include(m => m.Patient)
                 .Select(m => m.Patient)
                 .Distinct()
                 .ToListAsync();
            return Ok(uniquePatients);
        }

        [HttpGet("{id}")]
        public ActionResult<PatientReview> GetPatientReview(int id)
        {
            var patient = _context.PatientReviews.FirstOrDefault(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            return patient;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatientReview(int id, PatientReview patientReview)
        {
            if (id != patientReview.Id)
                return BadRequest();

            _context.Entry(patientReview).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientReviewExists(id))
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

        [HttpPost]
        public async Task<ActionResult<PatientReview>> PostPatientReview(PatientReview patientReview)
        {
            await _context.AddAsync(patientReview);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPatientReview", new { id = patientReview.Id }, patientReview);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatientReview(int id)
        {
            var result= await _context.PatientReviews.FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            _context.PatientReviews.Remove(result);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PatientReviewExists(int id)
        {
            return _context.PatientReviews.Any(e => e.Id == id);
        }
    }
}
