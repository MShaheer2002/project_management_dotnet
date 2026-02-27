using ProjectManagementBackend.Api.Extension;

var builder = WebApplication.CreateBuilder(args);


// services
builder.Services.AddSwaggerServices();
builder.Services.AddApiService(builder.Configuration);

var app = builder.Build();

app.UseApiPipeline();

app.Run();
