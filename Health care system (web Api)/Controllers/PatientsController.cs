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
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public PatientsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _dbContext.Patients
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .Select(p => new PatientResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    Appointments = p.Appointments
                        .Select(a => new AppointmentPatientResponse
                        {
                            DoctorName = a.Doctor.Name,
                            AppointmentDate = a.AppointmentDate
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(patients);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Getpatient(int id)
        {
            var patient = await _dbContext.Patients
                .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                return NotFound();

            var response = new PatientResponse
            {
                Id = patient.Id,
                Name = patient.Name,
                BirthDate = patient.BirthDate,
                Appointments = patient.Appointments
                    .Select(a => new AppointmentPatientResponse
                    {
                        DoctorName = a.Doctor.Name,
                        AppointmentDate = a.AppointmentDate
                    })
                    .ToList()
            };

            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePatient(CreatePatientsRequest patientRequest)
        {
            if (patientRequest is not null)
            {
                if (string.IsNullOrEmpty(patientRequest.Name))
                    return BadRequest("Category name is required.");
                if (patientRequest.BirthDate == default)
                    return BadRequest("Birth date is required.");

                var patient = new Patient
                {
                    Name = patientRequest.Name,
                    BirthDate = patientRequest.BirthDate
                };
                await _dbContext.Patients.AddAsync(patient);
                await _dbContext.SaveChangesAsync();
                return Created();
            }
            return BadRequest("Invalid patients data.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePatient(UpdatePatientsRequest patientRequest)
        {
            var patient = await _dbContext.Patients.FirstOrDefaultAsync(P => P.Id == patientRequest.Id);
            if (patient == null)
                return NotFound();
            patient.Name = patientRequest.Name;
            patient.BirthDate = patientRequest.BirthDate;
            _dbContext.Patients.Update(patient);
            await _dbContext.SaveChangesAsync();
            return Ok(patient);

        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
                return NotFound();
            _dbContext.Patients.Remove(patient);
            await _dbContext.SaveChangesAsync();
            return Ok(patient);
        }
    }
}
