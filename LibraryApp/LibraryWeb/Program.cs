using LibraryWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<BookApi>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055/");
});

builder.Services.AddHttpClient<CustomerApi>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055/");
});
builder.Services.AddHttpClient<CirculationApi>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055/");
});
builder.Services.AddHttpClient<ReportsApi>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055/");
});
builder.Services.AddHttpClient<ExhibitionsApi>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7055/");
});


// === Добавляем сессии ===
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// === Middleware сессии ===
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
