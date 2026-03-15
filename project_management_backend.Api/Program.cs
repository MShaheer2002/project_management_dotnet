using ProjectManagementBackend.Api.Extension;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load(Path.Combine(builder.Environment.ContentRootPath, ".env"));
builder.Configuration.AddEnvironmentVariables();


// services
builder.Services.AddSwaggerServices();
builder.Services.AddApiService(builder.Configuration);
builder.Services.AddCustomApiBehavior();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseApiPipeline();

app.Run();
