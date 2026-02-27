using project_management_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
        return services;
    }
}