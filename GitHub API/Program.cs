

using GitHub_API.CachedServices;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<GitHubIntegrationOptions>(builder.Configuration.GetSection(nameof(GitHubIntegrationOptions)));
builder.Services.AddGitHubIntegration(option => builder.Configuration.GetSection(nameof(GitHubIntegrationOptions)));

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IGitHubService, GitHubService>();
builder.Services.Decorate<IGitHubService, CachedGitHubService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
