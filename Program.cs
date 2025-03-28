using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FYPIBDPatientApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Services.Interfaces;
using FYPIBDPatientApp.Services;
using Microsoft.OpenApi.Models;
using FYPIBDPatientApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register repository interfaces and implementations
builder.Services.AddScoped<IHydRepository, HydRepository>();
builder.Services.AddScoped<IDietRepository, DietRepository>();
builder.Services.AddScoped<IBmRepository, BmRepository>();
builder.Services.AddScoped<IApptRepository, ApptRepository>();
builder.Services.AddScoped<ILsRepository, LsRepository>();
builder.Services.AddScoped<ISympRepository, SympRepository>();
builder.Services.AddScoped<IPsRepository, PsRepository>();

// Register additional services
builder.Services.AddScoped<FYPIBDPatientApp.Services.TokenService>();
builder.Services.AddScoped<IHydService, HydService>();
builder.Services.AddScoped<IDietService, DietService>();
builder.Services.AddScoped<IBmService, BmService>();
builder.Services.AddScoped<IApptService, ApptService>();
builder.Services.AddScoped<ILsService, LsService>();
builder.Services.AddScoped<ISympService, SympService>();
builder.Services.AddScoped<IPsService, PsService>();

// Register the AppDbContext using SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register logging service
builder.Services.AddScoped<ILoggingService, LoggingService>();

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Add controllers
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FYPIBDPatientApp API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configure the host URLs before building the app.
builder.WebHost.UseUrls("http://0.0.0.0:5276", "https://0.0.0.0:7197");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
