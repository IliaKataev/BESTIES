using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Repositories;
using LibraryApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Подключаем контроллеры
builder.Services.AddControllers();

// Swagger (для тестирования API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Подключаем PostgreSQL
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// репозитории
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();
builder.Services.AddScoped<IExhibitionRepository, ExhibitionRepository>();

// Сервисы
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IExhibitionService, ExhibitionService>();

// Разрешаем запросы с веб-клиента (CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        policy => policy
            .WithOrigins("https://localhost:7287") // адрес твоего веба
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseCors("AllowWebApp");
app.UseAuthorization();

try
{
    app.MapControllers();
}
catch (ReflectionTypeLoadException ex)
{
    Console.WriteLine("LoaderExceptions:");
    foreach (var le in ex.LoaderExceptions)
        Console.WriteLine(le.Message);
}

app.Run();
