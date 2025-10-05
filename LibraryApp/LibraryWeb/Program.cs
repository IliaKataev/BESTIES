using LibraryWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Подключаем MVC (контроллеры + представления)
builder.Services.AddControllersWithViews();

// HttpClient для связи с API
builder.Services.AddHttpClient<BookApi>(client =>
{
    // URL API (порт должен совпадать с LibraryApi)
    client.BaseAddress = new Uri("https://localhost:7055/");
});

var app = builder.Build();

// Конфигурация пайплайна
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles(); // чтобы отдавать файлы из wwwroot


// Маршруты
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
