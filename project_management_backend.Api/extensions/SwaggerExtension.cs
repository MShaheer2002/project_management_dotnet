using Microsoft.OpenApi.Models;

namespace ProjectManagementBackend.Api.Extension;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Project Management API",
                Version = "v1",
                Description = "Backend API for project management system"
            });
        });

        return services;
    }
}