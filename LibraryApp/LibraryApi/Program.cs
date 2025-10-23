using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Repositories;
using LibraryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Подключаем контроллеры
builder.Services.AddControllers();

// Swagger (для тестирования API)
builder.Services.AddEndpointsApiExplorer();

// Подключаем PostgreSQL
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));

// Generic репозитории
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Специализированные репозитории
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();

// Сервисы
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICustomerService, CustomerService>(); // или CustomerService, если назовём так
builder.Services.AddScoped<IIssueService, IssueService>();

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


app.UseHttpsRedirection();
app.UseCors("AllowWebApp");
app.UseAuthorization();

app.MapControllers();

app.Run();
