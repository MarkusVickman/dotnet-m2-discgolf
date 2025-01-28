var builder = WebApplication.CreateBuilder(args);

// Aktiverar och lägger till mvc
builder.Services.AddControllersWithViews();

var app = builder.Build();

//Lägger till stöd för statiska filer och routing
app.UseStaticFiles();
app.UseRouting();

//Mappar routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
 