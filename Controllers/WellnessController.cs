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

        public WellnessController(ISympService sympService,
            IBmService bmService,
            IDietService dietService,
            IHydService hydService,
            ILsService lsService)
        {
            _sympService = sympService;
            _bmService = bmService;
            _dietService = dietService;
            _hydService = hydService;
            _lsService = lsService;
        }

        //-------------------------------------------------------------------BM

        //GET: api/wellness/bm/{id}
        [HttpGet("bm/{id}")]
        [Authorize]
        public async Task<IActionResult> GetBowelMovementLog(int id)
        {
            var log = await _bmService.GetBowelMovementLog(id);
            if (log == null)
            {
                return NotFound("Bowel movement log not found.");
            }
            return Ok(log);
        }

        //GET: api/wellness/bm
        [HttpGet("bm")]
        [Authorize]
        public async Task<IActionResult> GetBowelMovementLogs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var logs = await _bmService.GetBowelMovementLogsForPatient(userId);

            return Ok(logs);
        }

        //GET: api/wellness/bm/recap
        [HttpGet("bm/recap")]
        [Authorize]
        public async Task<IActionResult> GetBowelMovementRecapForPatient ()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            // this returns List<(int value, string label)>
            var recapTuples = await _bmService.GetBowelMovementRecapForPatient(userId);

            // map tuples into plain objects with the right JSON names
            var recap = recapTuples
                .Select(t => new
                {
                    value = t.value,
                    label = t.label
                })
                .ToList();

            return Ok(recap);
        }

        //DELETE: api/wellness/bm/{id}
        [HttpDelete("bm/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBowelMovementLog(int id)
        {
            await _bmService.DeleteBowelMovementLog(id);
            return NoContent();
        }

        //-------------------------------------------------------------------DIET

        //GET: api/wellness/diet/{id}
        [HttpGet("diet/{id}")]
        [Authorize]
        public async Task<IActionResult> GetDietaryLog(int id)
        {
            var log = await _dietService.GetDietaryLog(id);
            if (log == null)
            {
                return NotFound("Dietary log not found.");
            }
            return Ok(log);
        }

        //GET: api/wellness/diet
        [HttpGet("diet")]
        [Authorize]
        public async Task<IActionResult> GetDietaryLogs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var logs = await _dietService.GetDietaryLogsForPatient(userId);

            return Ok(logs);
        }

        [HttpGet("diet/recap")]
        [Authorize]
        public async Task<IActionResult> GetDietaryRecapForPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var logs = await _dietService.GetDietaryRecap(userId);

            return Ok(logs);
        }

        //DELETE: api/wellness/diet/{id}
        [HttpDelete("diet/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteDietaryLog(int id)
        {
            await _dietService.DeleteDietaryLog(id);
            return NoContent();
        }

        //-------------------------------------------------------------------HYD

        //GET: api/wellness/hyd/{id}
        //Retrieves a single hydration log by its unique ID.
        [HttpGet("hyd/{id}")]
        [Authorize]
        public async Task<IActionResult> GetHydrationLog(int id)
        {
            var log = await _hydService.GetHydrationLog(id);
            if (log == null)
            {
                return NotFound("Hydration log not found.");
            }
            return Ok(log);
        }

        // GET: api/wellness/hyd
        // Retrieves all hydration logs for the authenticated patient.
        [HttpGet("hyd")]
        [Authorize]
        public async Task<IActionResult> GetHydrationLogsForPatient()
        {
            // Extract the patient ID from the token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found in token.");
            }

            var logs = await _hydService.GetHydrationLogsForPatient(userId);
            return Ok(logs);
        }

        // GET: api/wellness/hyd/date/{date}
        // Retrieves hydration recap for the past 7 days.
        [HttpGet("hyd/recap")]
        [Authorize]
        public async Task<IActionResult> GetHydrationRecapForPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found in token.");


            (double todayTotal, double pastSixTotal) = await _hydService.GetHydrationTotalsForPatient(userId);

            return Ok(new
            {
                todayIntake = todayTotal,
                pastSixDaysIntake = pastSixTotal
            });
        }

        //DELETE: api/wellness/diet/{id}
        [HttpDelete("hyd/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteHydrationLog(int id)
        {
            await _hydService.DeleteHydrationLog(id);
            return NoContent();
        }

        //-------------------------------------------------------------------LS

        //GET: api/wellness/ls/{id}
        [HttpGet("ls/{id}")]
        [Authorize]
        public async Task<IActionResult> GetLifestyleLog(int id)
        {
            var log = await _lsService.GetLifestyleLog(id);
            if (log == null)
            {
                return NotFound("Lifestyle log not found.");
            }
            return Ok(log);
        }

        //GET: api/wellness/ls
        [HttpGet("ls")]
        [Authorize]
        public async Task<IActionResult> GetLifestyleLogs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var logs = await _lsService.GetLifestyleLogsForPatient(userId);

            return Ok(logs);
        }

        //GET: api/wellness/ls/slrecap
        [HttpGet("ls/slrecap")]
        [Authorize]
        public async Task<IActionResult> GetSleepRecapForPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var recapTuples = await _lsService.GetSleepRecapForPatient(userId);

            var recap = recapTuples
                .Select(t => new
                {
                    value = t.value,
                    label = t.label
                })
                .ToList();

            return Ok(recap);
        }

        //GET: api/wellness/ls/exrecap
        [HttpGet("ls/exrecap")]
        [Authorize]
        public async Task<IActionResult> GetExerciseRecapForPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var recapTuples = await _lsService.GetExerciseRecapForPatient(userId);

            var recap = recapTuples
                .Select(t => new
                {
                    value = t.value,
                    label = t.label
                })
                .ToList();

            return Ok(recap);
        }

        //GET: api/wellness/ls/mrecap
        [HttpGet("ls/mrecap")]
        [Authorize]
        public async Task<IActionResult> GetMoodRecapForPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var recapTuples = await _lsService.GetMoodRecapForPatient(userId);

            var recap = recapTuples
                .Select(t => new
                {
                    value = t.value,
                    label = t.label
                })
                .ToList();

            return Ok(recap);
        }

        //DELETE: api/wellness/ls/{id}
        [HttpDelete("ls/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLifestyleLog(int id)
        {
            await _lsService.DeleteLifestyleLog(id);
            return NoContent();
        }

        //-------------------------------------------------------------------SYMP

        //GET: api/wellness/symp/{id}
        [HttpGet("symp/{id}")]
        [Authorize]
        public async Task<IActionResult> GetSymptomLog(int id)
        {
            var log = await _sympService.GetSymptomLog(id);
            if (log == null)
            {
                return NotFound("Symptom log not found.");
            }
            return Ok(log);
        }

        //GET: api/wellness/symp
        [HttpGet("symp")]
        [Authorize]
        public async Task<IActionResult> GetSymptomLogs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var logs = await _sympService.GetSymptomLogsForPatient(userId);

            return Ok(logs);
        }

        //GET: api/wellness/symp/recap
        [HttpGet("symp/recap")]
        [Authorize]
        public async Task<IActionResult> GetSymptomRecapForPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            // this returns List<(int value, string label)>
            var recapTuples = await _sympService.GetSymptomRecapForPatient(userId);

            // map tuples into plain objects with the right JSON names
            var recap = recapTuples
                .Select(t => new
                {
                    value = t.value,
                    label = t.label
                })
                .ToList();

            return Ok(recap);
        }


        //DELETE: api/wellness/symp/{id}
        [HttpDelete("symp/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSymptomLog(int id)
        {
            await _sympService.DeleteSymptomLog(id);
            return NoContent();
        }
    }
}
