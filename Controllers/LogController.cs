using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Security.Claims;

namespace FYPIBDPatientApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILoggingService _logger;

        public LogController(AppDbContext context, ILoggingService loggingService)
        {
            _context = context;
            _logger = loggingService;
        }


        [HttpPost("logBM")]
        [Authorize]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> LogBowelMovement([FromBody] BowelMovementLogDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var bowelMovementLog = new BowelMovementLog
            {
                PatientId = userId,
                Date = DateTime.UtcNow,
                StoolType = dto.StoolType,
                Blood = dto.Blood,
                Urgency = dto.Urgency,
                Notes = dto.Notes
                
            };

            _context.BowelMovementLogs.Add(bowelMovementLog);
            await _context.SaveChangesAsync();

            //logging
            await _logger.LogActionAsync(userId, "LogBowelMovement");
            return CreatedAtAction(nameof(GetBowelMovementLog), new { id = bowelMovementLog.Id }, bowelMovementLog);

        }

        [HttpPost("logD")]
        [Authorize]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> LogDiet([FromBody] DietaryLogDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var dietaryLog = new DietaryLog
            {
                PatientId = userId,

            }

        }

        [HttpPost("logH")]
        [Authorize]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> LogHydration([FromBody] HydrationLogDto dto)
        {

        }

        [HttpPost("LogL")]
        [Authorize]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> LogLifestyle([FromBody] LifestyleLogDto dto)
        {

        }

        [HttpPost("LogS")]
        [Authorize]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> LogSymptom([FromBody] SymptomLogDto dto)
        {

        }

    }
}
