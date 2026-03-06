using project_management_backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using project_management_backend.Infrastructure.Repository;
using project_management_backend.Application.Repository;

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

        return services;
    }
}