using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Repositories;
using LibraryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// ����� �������� �����������
builder.Services.AddControllers();

// ������������ � PostgreSQL
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));

// ������������ ����������� (����������� � �������)
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookService>();

// ��������� ������� � ������� (CORS)
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

// ���������� ���� ����� CORS
app.UseCors("AllowWebClient");

// ���������� �������� ������������
app.MapControllers();

app.Run();
