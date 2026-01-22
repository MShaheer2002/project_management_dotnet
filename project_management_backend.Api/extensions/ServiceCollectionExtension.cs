namespace ProjectManagementBackend.Api.Extension;

public static class ServiceCollectionExtension
{

    public static IServiceCollection AddApiService(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        return services;
    }
}