using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));

var app = builder.Build();
app.MapControllers();
app.Run();
