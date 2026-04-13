using Health_care_system__web_Api_.Data.Interceptor;
using Health_care_system__web_Api_.Data;
using Microsoft.EntityFrameworkCore;
using Health_care_system__web_Api_.Rrpositories;
using Health_care_system__web_Api_.Services;

namespace Health_care_system__web_Api_.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(Program));
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<ApplicationDbContext>(options =>

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(new SoftDeleteInterceptor())
                .AddInterceptors(new CreatedAtInceptor())
                );
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IDoctorService,DoctorService>();
            services.AddScoped<IPatientService,PatientService>();
            return services;
            

        }
    }
}
