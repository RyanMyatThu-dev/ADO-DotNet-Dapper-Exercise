using Book_Management.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// HTTP client for the Book Management API
builder.Services.AddHttpClient<IBookApiService, BookApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Api:BaseUrl")
        ?? "http://localhost:5203");
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "provider-books",
    pattern: "{provider}/{controller=Books}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
