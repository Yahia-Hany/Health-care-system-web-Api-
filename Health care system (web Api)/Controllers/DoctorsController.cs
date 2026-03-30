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
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMapper _mapper;

        public DoctorsController(ApplicationDbContext dbContext, ILogger<DoctorsController> logger,IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors([FromQuery] QuerryParams request)
        {
            _logger.LogInformation("Fetching doctors with search: {Search}, page: {Page}", request.Search, request.Page);

            var query = _dbContext.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();

                query = query.Where(d =>
                    d.Name.ToLower().Contains(search) ||
                    d.Specialization.ToLower().Contains(search));
            }


            var paginated = PaginationHelper
                .ApplyPagination(query, request.Page, request.PageSize);

            var result = await paginated.ToListAsync();
            var response = _mapper.Map<List<DoctorResponse>>(result);

            _logger.LogInformation("Returned {Count} doctors", result.Count);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            _logger.LogInformation("Fetching doctor with Id: {Id}", id);

            var doctor = await _dbContext.Doctors
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found with Id: {Id}", id);
                return NotFound();
            }

            var response = _mapper.Map<DoctorResponse>(doctor);

            _logger.LogInformation("Doctor returned with Id: {Id}", id);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor(CreateDoctorsRequest doctorsRequest)
        {
            _logger.LogInformation("CreateDoctor called with Name: {Name}", doctorsRequest?.Name);

            if (doctorsRequest is not null)
            {
                if (string.IsNullOrEmpty(doctorsRequest.Name))
                {
                    _logger.LogWarning("Doctor creation failed: Name is empty");
                    return BadRequest("Doctor name is required.");
                }

                if (string.IsNullOrEmpty(doctorsRequest.Specialization))
                {
                    _logger.LogWarning("Doctor creation failed: Specialization is empty");
                    return BadRequest("Specialization is required.");
                }

                var doctor = _mapper.Map<Doctor>(doctorsRequest);

                await _dbContext.Doctors.AddAsync(doctor);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Doctor created with Id: {Id}", doctor.Id);

                return Created();
            }

            _logger.LogWarning("CreateDoctor failed: request is null");
            return BadRequest("Invalid Doctor data.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDoctors(UpdateDoctorRequest doctorRequest)
        {
            _logger.LogInformation("Updating doctor with Id: {Id}", doctorRequest.Id);

            var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == doctorRequest.Id);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found for update with Id: {Id}", doctorRequest.Id);
                return NotFound();
            }

            _mapper.Map(doctorRequest, doctor);

            _dbContext.Doctors.Update(doctor);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Doctor updated with Id: {Id}", doctorRequest.Id);

            return Ok(doctor);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            _logger.LogInformation("Deleting doctor with Id: {Id}", id);

            var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(D => D.Id == id);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found for deletion with Id: {Id}", id);
                return NotFound();
            }

            _dbContext.Doctors.Remove(doctor);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Doctor deleted with Id: {Id}", id);

            return Ok(doctor);
        }
    }
}