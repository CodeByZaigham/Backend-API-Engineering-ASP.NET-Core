using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using shopforge_backend.config;
using shopforge_backend.iservices;
using shopforge_backend.services;

var builder = WebApplication.CreateBuilder(args);

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<appdbcontext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
builder.Services.AddScoped<iauthservice,authservice>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//swagger
    app.UseSwaggerUI();//swagger
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization(); //swagger
app.MapControllers();
app.Run();
