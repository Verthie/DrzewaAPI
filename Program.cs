global using DrzewaAPI.Dtos;
global using DrzewaAPI.Middleware.Exceptions;
global using DrzewaAPI.Models.Enums;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Azure.Storage.Blobs;
using DotNetEnv;
using DrzewaAPI.Configuration;
using DrzewaAPI.Data;
using DrzewaAPI.Middleware;
using DrzewaAPI.Models;
using DrzewaAPI.Services;
using DrzewaAPI.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add appsettings.json + environment variables
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Azure Storage Configuration
builder.Services.AddSingleton(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("AzureStorage");
    return new BlobServiceClient(connectionString);
});

// Service Registration
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();
builder.Services.AddScoped<ITreeService, TreeService>();
builder.Services.AddScoped<ISpeciesService, SpeciesService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IApplicationTemplateService, ApplicationTemplateService>();
builder.Services.AddScoped<ICommuneService, CommuneService>();
builder.Services.AddScoped<IFileGenerationService, FileGenerationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Validators
builder.Services.AddScoped<IValidator<UpdatePasswordDto>, ResetPasswordValidator>();

// Background Services
builder.Services.AddHostedService<TokenCleanupService>();

// API Connections
builder.Services.AddHttpClient<IGeoportalService, GeoportalService>();

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
         {
             // Stop the default 401 response from being sent
             context.HandleResponse();

             // Set the status code to 401
             context.Response.StatusCode = 401;
             context.Response.ContentType = "application/json";

             // Write the custom message to the response body
             var responseBody = JsonSerializer.Serialize(new { message = "You are not authorized to access this resource." });
             return context.Response.WriteAsync(responseBody);
         }
    };
});

builder.Services.AddAuthorization();

// Controller Configuration
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    // options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FastPolicy", options =>
    {
        options.PermitLimit = 1;
        options.Window = TimeSpan.FromSeconds(1);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    options.AddFixedWindowLimiter("UserPolicy", options =>
    {
        options.PermitLimit = 2;
        options.Window = TimeSpan.FromSeconds(5);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    options.AddFixedWindowLimiter("SlowPolicy", options =>
    {
        options.PermitLimit = 3;
        options.Window = TimeSpan.FromSeconds(6);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT token below (no need to add 'Bearer ' prefix)"
    };

    c.AddSecurityDefinition("Bearer", jwtScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Drzewa API V1");
        c.RoutePrefix = "";
    }); ;
}

// Replace or modify your HTTPS redirection
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// Only use HTTPS redirection if not in a container
if (!string.IsNullOrEmpty(builder.Configuration["App:BaseUrl"])
    || app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();