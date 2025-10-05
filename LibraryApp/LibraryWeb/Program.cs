using LibraryWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// ���������� MVC (����������� + �������������)
builder.Services.AddControllersWithViews();

// HttpClient ��� ����� � API
builder.Services.AddHttpClient<BookApi>(client =>
{
    // URL API (���� ������ ��������� � LibraryApi)
    client.BaseAddress = new Uri("https://localhost:7055/");
});

var app = builder.Build();

// ������������ ���������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles(); // ����� �������� ����� �� wwwroot


// ��������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
