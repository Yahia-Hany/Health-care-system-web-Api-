using System.Numerics;
using AutoMapper;
using Health_care_system__web_Api_.Data;
using Health_care_system__web_Api_.Dtos;
using Health_care_system__web_Api_.Entities;
using Health_care_system__web_Api_.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_care_system__web_Api_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IMapper _mapper;


        public AppointmentsController(ApplicationDbContext dbContext, ILogger<AppointmentsController> logger,IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments([FromQuery] QuerryParams request)
        {
            _logger.LogInformation("Fetching appointments with search: {Search}, page: {Page}", request.Search, request.Page);

            var query = _dbContext.Appointments.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();

                query = query.Where(a =>
                    a.Doctor.Name.ToLower().Contains(search) ||
                    a.Patient.Name.ToLower().Contains(search));
            }

            var paginated = PaginationHelper
                .ApplyPagination(query, request.Page, request.PageSize);

            var result = await paginated.ToListAsync();

            _logger.LogInformation("Returned {Count} appointments", result.Count);
            var response = _mapper.Map<List<AppointmentResponse>>(result);

            return Ok(result);
        }

        [HttpGet("{doctorid:int}/{patientid:int}")]
        public async Task<IActionResult> GetAppointment(int doctorid, int patientid)
        {
            _logger.LogInformation("Fetching appointment with DoctorId: {DoctorId}, PatientId: {PatientId}", doctorid, patientid);

            var appointment = await _dbContext.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.DoctorId == doctorid && a.PatientId == patientid);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found with DoctorId: {DoctorId}, PatientId: {PatientId}", doctorid, patientid);
                return NotFound();
            }

            var response = _mapper.Map<AppointmentResponse>(appointment);
            _logger.LogInformation("Appointment returned for DoctorId: {DoctorId}, PatientId: {PatientId}", doctorid, patientid);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentsRequest appointmentRequest)
        {
            _logger.LogInformation("CreateAppointment called with DoctorId: {DoctorId}, PatientId: {PatientId}",
                appointmentRequest?.DoctorId, appointmentRequest?.PatientId);

            if (appointmentRequest is not null)
            {
                if (!await _dbContext.Doctors.AnyAsync(d => d.Id == appointmentRequest.DoctorId))
                {
                    _logger.LogWarning("Appointment creation failed: Doctor not found with Id: {Id}", appointmentRequest.DoctorId);
                    return BadRequest("Doctor not found.");
                }

                if (!await _dbContext.Patients.AnyAsync(p => p.Id == appointmentRequest.PatientId))
                {
                    _logger.LogWarning("Appointment creation failed: Patient not found with Id: {Id}", appointmentRequest.PatientId);
                    return BadRequest("Patient not found.");
                }

                if (appointmentRequest.AppointmentDate == default)
                {
                    _logger.LogWarning("Appointment creation failed: Invalid date");
                    return BadRequest("Appointment date is required.");
                }
                var appointment = _mapper.Map<Appointment>(appointmentRequest);
                await _dbContext.Appointments.AddAsync(appointment);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Appointment created with DoctorId: {DoctorId}, PatientId: {PatientId}",
                    appointment.DoctorId, appointment.PatientId);

                return Created();
            }

            _logger.LogWarning("CreateAppointment failed: request is null");

            return BadRequest("Invalid appointment data.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppointment(updateAppointmentsRequest request)
        {
            _logger.LogInformation("Updating appointment with DoctorId: {DoctorId}, PatientId: {PatientId}",
                request.DoctorId, request.PatientId);

            var appointment = await _dbContext.Appointments
                .FirstOrDefaultAsync(a => a.DoctorId == request.DoctorId
                                      && a.PatientId == request.PatientId);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found for update with DoctorId: {DoctorId}, PatientId: {PatientId}",
                    request.DoctorId, request.PatientId);
                return NotFound();
            }

            if (request.AppointmentDate == default)
            {
                _logger.LogWarning("Update failed: Invalid appointment date");
                return BadRequest("Appointment date is required.");
            }

            _mapper.Map(request, appointment);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Appointment updated for DoctorId: {DoctorId}, PatientId: {PatientId}",
                request.DoctorId, request.PatientId);

            return Ok(appointment);
        }

        [HttpDelete("{doctorid:int}/{patientid:int}")]
        public async Task<IActionResult> DeleteAppointment(int doctorid, int patientid)
        {
            _logger.LogInformation("Deleting appointment with DoctorId: {DoctorId}, PatientId: {PatientId}", doctorid, patientid);

            var appointment = await _dbContext.Appointments
                .FirstOrDefaultAsync(a => a.PatientId == patientid && a.DoctorId == doctorid);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found for deletion with DoctorId: {DoctorId}, PatientId: {PatientId}", doctorid, patientid);
                return NotFound();
            }

            _dbContext.Appointments.Remove(appointment);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Appointment deleted with DoctorId: {DoctorId}, PatientId: {PatientId}", doctorid, patientid);

            return Ok(appointment);
        }
    }
}