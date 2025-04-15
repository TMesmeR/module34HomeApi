using FluentValidation;
using FluentValidation.AspNetCore;
using HomeApi;
using HomeApi.Configuration;
using HomeApi.Contracts.Validation;
using HomeApi.Data;
using HomeApi.Data.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем JSON-файлы конфигурации
builder.Configuration
    .AddJsonFile("HomeOptions.json", optional: true, reloadOnChange: true)
    .AddJsonFile("ConnectDbConf.json", optional: true, reloadOnChange: true);

// Регистрируем AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Регистрируем репозитории
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

// Настраиваем контекст базы данных
builder.Services.AddDbContextPool<HomeApiContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрируем валидаторы FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AddRoomRequestValidator>()
    .AddFluentValidationAutoValidation();

// Настраиваем конфигурационные модели
builder.Services.Configure<HomeOptions>(builder.Configuration);
builder.Services.Configure<Adress>(builder.Configuration.GetSection("Adress"));
builder.Services.AddControllers();

// Настройка Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c 
    => c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeApi", Version = "v1" }));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeApi v1"));
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();

