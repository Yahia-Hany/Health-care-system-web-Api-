using Health_care_system__web_Api_.Data;
using Health_care_system__web_Api_.Dtos;
using Health_care_system__web_Api_.Entities;
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

        public AppointmentsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var appointment = await _dbContext.Appointments
                .Select(a => new AppointmentResponse
                {
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.Name,
                    AppointmentDate = a.AppointmentDate
                })
            .ToListAsync();

            return Ok(appointment);
        }
        [HttpGet("{doctorid:int}/{patientid:int}")]
        public async Task<IActionResult> GetAppointment(int doctorid,int patientid)
        {
            var appointment = await _dbContext.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.DoctorId == doctorid&&a.PatientId == patientid);

            if (appointment == null)
                return NotFound();

            var response = new AppointmentResponse
            {
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.Name,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.Name,
                AppointmentDate = appointment.AppointmentDate
                    
            };

            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentsRequest appointmentRequest)
        {
            if (appointmentRequest is not null)
            {
                if(!await _dbContext.Doctors.AnyAsync(d => d.Id == appointmentRequest.DoctorId))
                    return BadRequest("Doctor not found.");
                if(!await _dbContext.Patients.AnyAsync(p => p.Id == appointmentRequest.PatientId))
                    return BadRequest("Patient not found.");
                if(appointmentRequest.AppointmentDate == default)
                    return BadRequest("Appointment date is required.");

                var appointment = new Appointment
                {
                    AppointmentDate = appointmentRequest.AppointmentDate,
                    DoctorId = appointmentRequest.DoctorId,
                    PatientId = appointmentRequest.PatientId
                };
                await _dbContext.Appointments.AddAsync(appointment);
                await _dbContext.SaveChangesAsync();
                return Created();
            }
            return BadRequest("Invalid appointment data.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAppointment(updateAppointmentsRequest request)
        {
            var appointment = await _dbContext.Appointments
                .FirstOrDefaultAsync(a => a.DoctorId == request.DoctorId
                                      && a.PatientId == request.PatientId);

            if (appointment == null)
                return NotFound();

            if (request.AppointmentDate == default)
                return BadRequest("Appointment date is required.");

            appointment.AppointmentDate = request.AppointmentDate;

            await _dbContext.SaveChangesAsync();

            return Ok(appointment);
        }
        [HttpDelete("{doctorid:int}/{patientid:int}")]
        public async Task<IActionResult> DeleteAppointment(int doctorid , int patientid)
        {
            var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(a => a.PatientId == patientid && a.DoctorId == doctorid);
            if (appointment == null)
                return NotFound();
            _dbContext.Appointments.Remove(appointment);
            await _dbContext.SaveChangesAsync();
            return Ok(appointment);
        }
    }
}
