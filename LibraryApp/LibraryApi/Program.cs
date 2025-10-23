using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Repositories;
using LibraryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ���������� �����������
builder.Services.AddControllers();

// Swagger (��� ������������ API)
builder.Services.AddEndpointsApiExplorer();

// ���������� PostgreSQL
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));

// Generic �����������
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// ������������������ �����������
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();

// �������
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICustomerService, CustomerService>(); // ��� CustomerService, ���� ������ ���
builder.Services.AddScoped<IIssueService, IssueService>();

// ��������� ������� � ���-������� (CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        policy => policy
            .WithOrigins("https://localhost:7287") // ����� ������ ����
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
