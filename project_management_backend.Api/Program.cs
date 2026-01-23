using ProjectManagementBackend.Api.Extension;

var builder = WebApplication.CreateBuilder(args);


// services
builder.Services.AddSwaggerServices();
builder.Services.AddApiService();

var app = builder.Build();

app.UseApiPipeline();

app.Run();
