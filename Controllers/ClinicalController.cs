using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FYPIBDPatientApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicalController : ControllerBase
    {
        private readonly IApptService _apptService;
        private readonly IPsService _psService;

        public ClinicalController(IApptService apptService, IPsService psService)
        {
            _apptService = apptService;
            _psService = psService;
        }

        //-------------------------------------------------------------------APPT

        //GET: api/clinical/appt/{id}
        [HttpGet("appt/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _apptService.GetAppointment(id);
            if (appointment == null)
            {
                return NotFound("Bowel movement log not found.");
            }
            return Ok(appointment);
        }

        //GET: api/clinical/appt
        [HttpGet("appt")]
        [Authorize]
        public async Task<IActionResult> GetAppointments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var appointments = await _apptService.GetAppointmentsForPatient(userId);

            return Ok(appointments);
        }

        //GET: api/clinical/appt/f
        [HttpGet("appt/f")]
        [Authorize]
        public async Task<IActionResult> GetFutureAppointments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var appointments = await _apptService.GetFutureAppointmentsForPatient(userId);

            return Ok(appointments);
        }

        //POST: api/clinical/appt
        [HttpPost("appt")]
        [Authorize]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _apptService.RecordAppointment(dto, userId);
            return Ok("Appointment added successfully.");
        }

        //DELETE: api/clinical/appt/{id}
        [HttpDelete("appt/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _apptService.DeleteAppointment(id);
            return NoContent();
        }

        //-------------------------------------------------------------------PS

        //GET: api/clinical/ps/{id}
        [HttpGet("ps/{id}")]
        [Authorize]
        public async Task<IActionResult> GetPrescription(int id)
        {
            var prescription = await _psService.GetPrescription(id);
            if (prescription == null)
            {
                return NotFound("Bowel movement log not found.");
            }
            return Ok(prescription);
        }

        //GET: api/clinical/ps
        [HttpGet("ps")]
        [Authorize]
        public async Task<IActionResult> GetPrescriptionsForPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var prescriptions = await _psService.GetPrescriptionsForPatient(userId);
            return Ok(prescriptions);
        }

        //GET: api/clinical/ps/date/{date}
        [HttpGet("ps/date/{date}")]
        [Authorize]
        public async Task<IActionResult> GetPrescriptionsForPatientOnDate(DateTime date)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found in token.");
            }

            var prescriptions = await _psService.GetPrescriptionsForPatientOnDate(userId, date);
            return Ok(prescriptions);
        }

        //GET: api/clinical/ps/c/
        [HttpGet("ps/c")]
        [Authorize]
        public async Task<IActionResult> GetCurrentPrescriptionsForPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var prescriptions = await _psService.GetCurrentPrescriptionsForPatient(userId);
            return Ok(prescriptions);
        }

        //POST: api/clinical/ps
        [HttpPost("ps")]
        [Authorize]
        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found in token.");
            }

            await _psService.NewPrescription(dto, userId);
            return Ok("Prescription added successfully.");
        }

        [HttpPut("ps")]
        [Authorize]
        public async Task<IActionResult> UpdatePrescription([FromBody] PrescriptionDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found in token.");
            }

            await _psService.ModifyPrescription(dto, userId);
            return Ok("Prescription updated successfully.");
        }

        [HttpDelete("ps/{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            await _psService.DeletePrescription(id);
            return NoContent();
        }
    }
}
