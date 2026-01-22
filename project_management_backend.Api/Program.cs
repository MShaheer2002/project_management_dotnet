using ProjectManagementBackend.Api.Extension;

var builder = WebApplication.CreateBuilder(args);


// services
builder.Services.AddSwaggerServices();
