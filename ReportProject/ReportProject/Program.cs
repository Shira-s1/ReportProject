//using ReportProject.Core.Interfaces;
//using ReportProject.Data;
//using ReportProject.Service.Service;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//var app = builder.Build();
//builder.Services.AddDbContext<DataContext>();
//builder.Services.AddScoped<IEmployeeService, EmployeeService>();
//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();

//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
//*************************************************
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API Name", Version = "v1" });
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
//    });
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
//***********************************************
//using Microsoft.AspNetCore.Cors.Infrastructure;
//using ReportProject.Core.Interfaces;
//using ReportProject.Data;
//using ReportProject.Service.Service;

//try
//{
//    var builder = WebApplication.CreateBuilder(args);

//    // Add services to the container.
//    builder.Services.AddControllers();
//    builder.Services.AddSwaggerGen(c =>
//    {
//        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API Name", Version = "v1" });
//    });

//    var app = builder.Build();
//    builder.Services.AddDbContext<DataContext>();
//    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
//    // Configure the HTTP request pipeline.
//    if (app.Environment.IsDevelopment())
//    {
//        app.UseSwagger();
//        app.UseSwaggerUI(c =>
//        {
//            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
//        });
//    }

//    app.UseHttpsRedirection();
//    app.UseAuthorization();
//    app.MapControllers();
//    app.Run();
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"An error occurred during startup: {ex}");
//    // ���� �� ����� ���� �� �-Event Viewer ���
//}
//**************************************************


//using Microsoft.EntityFrameworkCore;
//using ReportProject.Core.DTOs;
//using ReportProject.Core.Interfaces;
//using ReportProject.Data;
//using ReportProject.Service.Service;

//var builder = WebApplication.CreateBuilder(args);

//// ���� �� �-Connection String �- appsettings.json
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//// ����� �- DbContext �� �- Connection String
//builder.Services.AddDbContext<DataContext>(options =>
//    options.UseSqlServer(connectionString));

//// ����� ��������
//builder.Services.AddControllers();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API Name", Version = "v1" });
//});

//// ����� ������ IEmployeeService
//builder.Services.AddScoped<IEmployeeService, EmployeeService>();

//builder.Services.AddAutoMapper(typeof(MapperProfile));
//var app = builder.Build();

//// ����� Swagger �� ������ �����
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
//    });
//}

//// ����� ���� ������
//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();
//app.Run();

using Microsoft.EntityFrameworkCore;
using ReportProject.Core.DTOs;
using ReportProject.Core.Interfaces;
using ReportProject.Data;
using ReportProject.Service.Service;

var builder = WebApplication.CreateBuilder(args);

// ����� DbContext �� �- ConnectionString (��� ��� ����!)
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����� ������� ������
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEnryAndExitService, EntryAndExitService>();
builder.Services.AddScoped<IVacationsService, VacationsService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddControllers();

// ����� Swagger �� �� ����
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API Name", Version = "v1" });
});

var app = builder.Build();

// ����������� ���� Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
// ������� ����� ��� ������ - ���� �� ������ ��� 
//The ConnectionString property has not been initialized.