using System.Numerics;
using AutoMapper;
using Azure;
using Health_care_system__web_Api_.Data;
using Health_care_system__web_Api_.Dtos;
using Health_care_system__web_Api_.Entities;
using Health_care_system__web_Api_.Helpers;
using Health_care_system__web_Api_.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_care_system__web_Api_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        //private readonly ApplicationDbContext _dbContext;
        private readonly IPatientService _service;
        private readonly ILogger<PatientsController> _logger;
        private readonly IMapper _mapper;

        public PatientsController(IPatientService service, ILogger<PatientsController> logger,IMapper mapper)
        {
            _service = service;
            //_dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            _logger.LogInformation("Fetching all patients");
            var patients = await _service.GetAllAsync();
            var response = _mapper.Map<List<PatientResponse>>(patients);
            _logger.LogInformation("Returned {Count} patients", response.Count);
            if (!response.Any())
            {
                _logger.LogWarning("No patients found");
                return NotFound("No patients found.");
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("GetById/{id:int}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            _logger.LogInformation("Fetching patient with Id: {Id}", id);
            var patient = await _service.GetByIdAsync(id);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found with Id: {Id}", id);
                return NotFound($"Patient with Id {id} not found.");
            }
            var response = _mapper.Map<PatientResponse>(patient);
            _logger.LogInformation("Patient returned with Id: {Id}", id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePatient(CreatePatientsRequest patientsRequest)
        {
            _logger.LogInformation("CreatePatient called with Name: {Name}", patientsRequest?.Name);
            if (patientsRequest is not null)
            {
                if (string.IsNullOrEmpty(patientsRequest.Name))
                {
                    _logger.LogWarning("Patient creation failed: Name is empty");
                    return BadRequest("Category name is required.");
                }
                if (patientsRequest.BirthDate == default)
                {
                    _logger.LogWarning("Patient creation failed: BirthDate is invalid");
                    return BadRequest("Birth date is required.");
                }
                var patient = _mapper.Map<Patient>(patientsRequest);
                var createdPatient = await _service.CreateAsync(patient);
                _logger.LogInformation("Patient created with Id: {Id}", createdPatient.Id);
                return CreatedAtAction(nameof(GetPatient), new { id = createdPatient.Id }, createdPatient);
            }
            _logger.LogWarning("CreatePatient failed: request is null");
            return BadRequest("Invalid patients data.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePatient(UpdatePatientsRequest patientsRequest)
        {
            _logger.LogInformation("Updating patient with Id: {Id}", patientsRequest.Id);
            var patient = await _service.GetByIdAsync(patientsRequest.Id);
            if (patient == null)
            {
                _logger.LogWarning("Patient not found for update with Id: {Id}", patientsRequest.Id);
                return NotFound();
            }
            _mapper.Map(patientsRequest, patient);
            var updateResult = await _service.UpdateAsync(patient);
            if (!updateResult)
            {
                _logger.LogError("Failed to update patient with Id: {Id}", patientsRequest.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update patient.");
            }
            _logger.LogInformation("Patient updated with Id: {Id}", patientsRequest.Id);
            return Ok(patient);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            _logger.LogInformation("Deleting patient with Id: {Id}", id);
            var deleteResult = await _service.DeleteAsync(id);
            if (!deleteResult)
            {
                _logger.LogWarning("Patient not found for deletion with Id: {Id}", id);
                return NotFound();
            }
            _logger.LogInformation("Patient deleted with Id: {Id}", id);
            return Ok($"Patient with Id {id} deleted successfully.");
        }

        //[HttpGet]
        //public async Task<IActionResult> GetPatients([FromQuery] QuerryParams request)
        //{
        //    _logger.LogInformation("Fetching patients with search: {Search}, page: {Page}", request.Search, request.Page);

        //    var query = _dbContext.Patients.AsQueryable();

        //    if (!string.IsNullOrEmpty(request.Search))
        //    {
        //        var search = request.Search.ToLower();

        //        query = query.Where(p =>
        //            p.Name.ToLower().Contains(search));
        //    }


        //    var paginated = PaginationHelper
        //        .ApplyPagination(query, request.Page, request.PageSize);

        //    var result = await paginated.ToListAsync();
        //    var response = _mapper.Map<List<PatientResponse>>(result);

        //    _logger.LogInformation("Returned {Count} patients", result.Count);

        //    return Ok(result);
        //}

        //[HttpGet("{id:int}")]
        //public async Task<IActionResult> Getpatient(int id)
        //{
        //    _logger.LogInformation("Fetching patient with Id: {Id}", id);

        //    var patient = await _dbContext.Patients
        //        .Include(p => p.Appointments)
        //        .ThenInclude(a => a.Doctor)
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (patient == null)
        //    {
        //        _logger.LogWarning("Patient not found with Id: {Id}", id);
        //        return NotFound();
        //    }

        //    var response = _mapper.Map<PatientResponse>(patient);

        //    _logger.LogInformation("Patient returned with Id: {Id}", id);

        //    return Ok(response);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreatePatient(CreatePatientsRequest patientRequest)
        //{
        //    _logger.LogInformation("CreatePatient called with Name: {Name}", patientRequest?.Name);

        //    if (patientRequest is not null)
        //    {
        //        if (string.IsNullOrEmpty(patientRequest.Name))
        //        {
        //            _logger.LogWarning("Patient creation failed: Name is empty");
        //            return BadRequest("Category name is required.");
        //        }

        //        if (patientRequest.BirthDate == default)
        //        {
        //            _logger.LogWarning("Patient creation failed: BirthDate is invalid");
        //            return BadRequest("Birth date is required.");
        //        }

        //        var patient = _mapper.Map<Patient>(patientRequest);

        //        await _dbContext.Patients.AddAsync(patient);
        //        await _dbContext.SaveChangesAsync();

        //        _logger.LogInformation("Patient created with Id: {Id}", patient.Id);

        //        return Created();
        //    }

        //    _logger.LogWarning("CreatePatient failed: request is null");

        //    return BadRequest("Invalid patients data.");
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdatePatient(UpdatePatientsRequest patientRequest)
        //{
        //    _logger.LogInformation("Updating patient with Id: {Id}", patientRequest.Id);

        //    var patient = await _dbContext.Patients.FirstOrDefaultAsync(P => P.Id == patientRequest.Id);

        //    if (patient == null)
        //    {
        //        _logger.LogWarning("Patient not found for update with Id: {Id}", patientRequest.Id);
        //        return NotFound();
        //    }

        //    _mapper.Map(patientRequest, patient);


        //    _dbContext.Patients.Update(patient);
        //    await _dbContext.SaveChangesAsync();

        //    _logger.LogInformation("Patient updated with Id: {Id}", patientRequest.Id);

        //    return Ok(patient);
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> DeletePatient(int id)
        //{
        //    _logger.LogInformation("Deleting patient with Id: {Id}", id);

        //    var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id);

        //    if (patient == null)
        //    {
        //        _logger.LogWarning("Patient not found for deletion with Id: {Id}", id);
        //        return NotFound();
        //    }

        //    _dbContext.Patients.Remove(patient);
        //    await _dbContext.SaveChangesAsync();

        //    _logger.LogInformation("Patient deleted with Id: {Id}", id);

        //    return Ok(patient);
        //}
    }
}