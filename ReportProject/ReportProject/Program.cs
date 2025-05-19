////use JWT Authentication
//using Microsoft.EntityFrameworkCore;
//using ReportProject.Core.DTOs;
//using ReportProject.Core.Interfaces;
//using ReportProject.Data;
//using ReportProject.Service.Service;
//using NLog;
//using NLog.Web;
//using System.IO;
//using ReportProject.Api.Middleware;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.OpenApi.Models;
//using Microsoft.AspNetCore.Identity;
//using ReportProject.Core.Entities;

//var builder = WebApplication.CreateBuilder(args);

//// Create Logs directory if it doesn't exist
//string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
//if (!Directory.Exists(logDirectory))
//{
//    Directory.CreateDirectory(logDirectory);
//    Console.WriteLine("Logs directory created at: " + logDirectory);  // Debug log
//}
//else
//{
//    Console.WriteLine("Logs directory already exists at: " + logDirectory);  // Debug log
//}

//// Configure NLog-------------------------------------
//LogManager.Setup().LoadConfigurationFromFile("nlog.config");
//builder.Logging.ClearProviders();
//builder.Host.UseNLog();

//builder.Services.AddDbContext<DataContext>();
//builder.Services.AddScoped<IEmployeeService, EmployeeService>();
//builder.Services.AddScoped<IReportService, ReportService>();
//builder.Services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();//AAA-JWT
//builder.Services.AddAutoMapper(typeof(MapperProfile));

//// Add services for Authentication and JWT
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        ClockSkew = TimeSpan.Zero,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));

//});

//// Add Swagger support for JWT
//builder.Services.AddOpenApi();
//builder.Services.AddSwaggerGen(setup =>
//{
//    var jwtSecurityScheme = new OpenApiSecurityScheme
//    {
//        BearerFormat = "JWT",
//        Name = "JWT Authentication",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = JwtBearerDefaults.AuthenticationScheme,
//        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

//        Reference = new OpenApiReference
//        {
//            Id = JwtBearerDefaults.AuthenticationScheme,
//            Type = ReferenceType.SecurityScheme
//        }
//    };

//    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

//    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        { jwtSecurityScheme, Array.Empty<string>() }
//    });
//});//AAAA
//builder.Services.AddSwaggerGen();//AAAA

//// Add services to the container.
//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
//    await employeeService.EnsureAdminUserExistsAsync();
//}
//// Middleware לטיפול בשגיאות
//app.UseMiddleware<ErrorMiddleware>();

//// Middleware ללוגינג
//app.UseMiddleware<LogMiddleware>();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.UseSwagger();//AAAA
//    app.UseSwaggerUI();//AAAA
//}

//app.UseHttpsRedirection();

//app.UseAuthentication(); // הוספת Middleware של Authentication
//app.UseAuthorization(); // הוספת Middleware של Authorization

//app.MapControllers();

//app.Run();
//use JWT Authentication
using Microsoft.EntityFrameworkCore;
using ReportProject.Core.DTOs;
using ReportProject.Core.Interfaces;
using ReportProject.Data;
using ReportProject.Service.Service;
using NLog;
using NLog.Web;
using System.IO;
using ReportProject.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using ReportProject.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Create Logs directory if it doesn't exist
string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
    Console.WriteLine("Logs directory created at: " + logDirectory);  // Debug log
}
else
{
    Console.WriteLine("Logs directory already exists at: " + logDirectory);  // Debug log
}

// Configure NLog-------------------------------------
LogManager.Setup().LoadConfigurationFromFile("nlog.config");
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();//AAA-JWT
builder.Services.AddAutoMapper(typeof(MapperProfile));

// Add services for Authentication and JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));

});

// Add Swagger support for JWT
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});//AAAA
builder.Services.AddSwaggerGen();//AAAA

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// קרא לשירות כדי לוודא שאדמין קיים - **הוסף כאן**
using (var scope = app.Services.CreateScope())
{
    var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
    try
    {
        await employeeService.EnsureAdminUserExistsAsync();
    }
    catch (Exception ex)
    {
        // רשום את השגיאה באופן מפורט
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error while ensuring admin user exists during startup.");
        // ייתכן שתרצה לטפל בשגיאה בצורה אחרת, כגון עצירת האפליקציה
        // אם יצירת האדמין הראשוני קריטית.
    }
}

// Middleware לטיפול בשגיאות
app.UseMiddleware<ErrorMiddleware>();

// Middleware ללוגינג
app.UseMiddleware<LogMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();//AAAA
    app.UseSwaggerUI();//AAAA
}

app.UseHttpsRedirection();

app.UseAuthentication(); // הוספת Middleware של Authentication
app.UseAuthorization(); // הוספת Middleware של Authorization

app.MapControllers();

app.Run();