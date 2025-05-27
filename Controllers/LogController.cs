using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Services;
using FYPIBDPatientApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FYPIBDPatientApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ISympService _sympService;
        private readonly IBmService _bmService;
        private readonly IDietService _dietService;
        private readonly IHydService _hydService;
        private readonly ILsService _lsService;
        private readonly ILoggingService _logger;

        public LogController(ISympService sympService,
            IBmService bmService,
            IDietService dietService,
            IHydService hydService,
            ILsService lsService,
            ILoggingService loggingService)
        {
            _sympService = sympService;
            _bmService = bmService;
            _dietService = dietService;
            _hydService = hydService;
            _lsService = lsService;
            _logger = loggingService;
        }


        [HttpPost("logBm")]
        [Authorize]
        public async Task<IActionResult> LogBowelMovement([FromBody] BowelMovementLogDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _bmService.RecordBowelMovementLog(dto, userId);
            return Ok("Bowel movement log recorded successfully.");
        }

        [HttpPost("logDiet")]
        [Authorize]
        public async Task<IActionResult> LogDiet([FromBody] DietaryLogDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _dietService.RecordDietaryLog(dto, userId);
            return Ok("Dietary log recorded successfully.");
        }

        [HttpPost("logHyd")]
        [Authorize]
        public async Task<IActionResult> LogHydration([FromBody] HydrationLogDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _hydService.RecordHydrationLog(dto, userId);
            return Ok("Hydration log recorded successfully.");
        }

        [HttpPost("LogLs")]
        [Authorize]
        public async Task<IActionResult> LogLifestyle([FromBody] LifestyleLogDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _lsService.RecordLifestyleLog(dto, userId);
            return Ok("Lifestyle log recorded successfully.");
        }

        [HttpPost("LogSymp")]
        [Authorize]
        public async Task<IActionResult> LogSymptom([FromBody] SymptomLogDto dto)
        {
            // Get the patient id from the token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            // Call the service layer to record the symptom log
            await _sympService.RecordSymptomLog(dto, userId);
            return Ok("Symptom log recorded successfully.");
        }

    }
}
