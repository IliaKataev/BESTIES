using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Repositories;
using LibraryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// нужно добавить контроллеры
builder.Services.AddControllers();

// подключиться к PostgreSQL
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));

// регистрируем зависимости (репозитории и сервисы)
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookService>();

// разрешаем запросы с клиента (CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// подключаем этот самый CORS
app.UseCors("AllowWebClient");

// подключаем маршруты контроллеров
app.MapControllers();

app.Run();
