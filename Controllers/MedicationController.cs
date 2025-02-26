using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Security.Claims;

namespace FYPIBDPatientApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILoggingService _logger;

        public MedicationController(AppDbContext context, ILoggingService loggingService)
        {
            _context = context;
            _logger = loggingService;
        }

        //Post: api/medication/addmedication
        [HttpPost("addmedication")]
        [Authorize(Roles = "HCPro")]
        public async Task<IActionResult> AddMedication([FromBody] AddMedicationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medication = new Medication
            {
                Name = dto.Name,
                Description = dto.Description,
                Notes = dto.Notes,
            };

            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            //logging
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _logger.LogActionAsync(userId, "AddMedication");

            return CreatedAtAction(nameof(GetMedicationById), new { id = medication.Id }, medication);
        }

        //Get: api/medication/"id"
        [HttpGet("{id}")]
        [Authorize(Roles = "HCPro")]
        public async Task<IActionResult> GetMedicationById(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            //logging
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _logger.LogActionAsync(userId, "GetMedicationById");

            return Ok(medication);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "HCPro")]
        public async Task<IActionResult> UpdateMedication(int id, [FromBody] UpdateMedicationDto dto)
        {  
            if (id != dto.Id)
            {
                return BadRequest("ID Mismatch.");
            }

            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            medication.Name = dto.Name;
            medication.Description = dto.Description;
            medication.Notes = dto.Notes;

            _context.Medications.Update(medication);
            await _context.SaveChangesAsync();

            //logging
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _logger.LogActionAsync(userId, "UpdateMedication");

            return NoContent();
        }
    }
}
