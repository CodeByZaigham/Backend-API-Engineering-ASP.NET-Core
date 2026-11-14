using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using shopforge_backend.Infrastructure.config;
using shopforge_backend.Application.iservices;
using shopforge_backend.Application.services;
using Microsoft.AspNetCore.Mvc;
using shopforge_backend.generics;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT access token."
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// FIX: name now matches the README ("DefaultConnection")
builder.Services.AddDbContext<appdbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata=false;
    options.SaveToken=true;
    options.TokenValidationParameters=new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,
        ValidIssuer=builder.Configuration.GetValue<string>("JWT:Issuer"),
        ValidAudience=builder.Configuration.GetValue<string>("JWT:Audience"),
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Key")))
    };
});

// CHANGED: enums (shippingMethod, paymentMethod, status...) travel as text,
// e.g. "express" / "cod" / "Shipped", which is what the frontend sends.
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// NEW: allow the React dev server to call this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddScoped<iauthservice,authservice>();
builder.Services.AddScoped<iProductsService,ProductsService>();
builder.Services.AddScoped<iCartService,CartService>();
builder.Services.AddScoped<iCouponService,CouponService>();   // NEW
builder.Services.AddScoped<iOrderService,OrderService>();     // NEW

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//swagger
    app.UseSwaggerUI();//swagger
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");   // NEW: must come before authentication
app.UseAuthentication(); //jwt
app.UseAuthorization(); //swagger , jwt
app.MapControllers();

//test route
app.MapGet("/", () =>
{
    return Results.Ok("API is working successfully!");
});


app.Run();
