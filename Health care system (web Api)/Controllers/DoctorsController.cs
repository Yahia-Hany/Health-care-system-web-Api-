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
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public DoctorsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var doctor = await _dbContext.Doctors
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .Select(d => new DoctorResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    Specialization = d.Specialization,
                    Appointments = d.Appointments
                        .Select(a => new AppointmentDoctorResponse
                        {
                            PatientName = a.Patient.Name,
                            AppointmentDate = a.AppointmentDate
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(doctor);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _dbContext.Doctors
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
                return NotFound();

            var response = new DoctorResponse
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialization = doctor.Specialization,
                Appointments = doctor.Appointments
                    .Select(a => new AppointmentDoctorResponse
                    {
                        PatientName = a.Patient.Name,
                        AppointmentDate = a.AppointmentDate
                    })
                    .ToList()
            };

            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDoctor(CreateDoctorsRequest doctorsRequest)
        {
            if (doctorsRequest is not null)
            {
                if (string.IsNullOrEmpty(doctorsRequest.Name))
                    return BadRequest("Doctor name is required.");
                if (string.IsNullOrEmpty(doctorsRequest.Specialization))
                    return BadRequest("Specialization is required.");

                var doctor = new Doctor
                {
                    Name = doctorsRequest.Name,
                    Specialization = doctorsRequest.Specialization
                };
                await _dbContext.Doctors.AddAsync(doctor);
                await _dbContext.SaveChangesAsync();
                return Created();
            }
            return BadRequest("Invalid Doctor data.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDoctors(UpdateDoctorRequest doctorRequest)
        {
            var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == doctorRequest.Id);
            if (doctor == null)
                return NotFound();
            doctor.Name = doctorRequest.Name;
            doctor.Specialization = doctorRequest.Specialization;
            _dbContext.Doctors.Update(doctor);
            await _dbContext.SaveChangesAsync();
            return Ok(doctor);

        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(D => D.Id == id);
            if (doctor == null)
                return NotFound();
            _dbContext.Doctors.Remove(doctor);
            await _dbContext.SaveChangesAsync();
            return Ok(doctor);
        }
    }
}
