var builder = WebApplication.CreateBuilder(args);

// Aktiverar och lägger till mvc
builder.Services.AddControllersWithViews();

//Lagt till stöd för kakor
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

//Lägger till stöd för statiska filer och routing
app.UseStaticFiles();
app.UseRouting();

//Stöd för kakor
app.UseSession();

//Mappar routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
 