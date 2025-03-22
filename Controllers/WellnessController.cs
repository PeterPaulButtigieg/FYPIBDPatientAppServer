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
    public class WellnessController : ControllerBase
    {
        private readonly ISympService _sympService;
        private readonly IBmService _bmService;
        private readonly IDietService _dietService;
        private readonly IHydService _hydService;
        private readonly ILsService _lsService;
        private readonly ILoggingService _logger;

        public WellnessController(ISympService sympService,
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

        [HttpGet("logBm")]
        [Authorize]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> LogBowelMovement([FromBody] BowelMovementLogDto dto)
        {

        }
}
