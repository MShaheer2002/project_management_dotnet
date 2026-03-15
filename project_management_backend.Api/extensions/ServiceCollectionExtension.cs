using project_management_backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using project_management_backend.Infrastructure.Repository;
using project_management_backend.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using project_management_backend.Application.common.responses;
using project_management_backend.Application.Services;

namespace ProjectManagementBackend.Api.Extension;

public static class ServiceCollectionExtension
{

    public static IServiceCollection AddApiService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // EF Core DbContext for MySQL
        var connectionString = configuration.GetConnectionString("ProjectManagementConnectionString");

        services.AddDbContext<ProjectManagementDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtService,JwtService>();

        return services;
    }


    public static IServiceCollection AddCustomApiBehavior(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var error = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage)
                    );
                var response = new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Validation failed",
                    Errors = error
                };
                return new BadRequestObjectResult(response);

            };
        });
        return services;

    }
}