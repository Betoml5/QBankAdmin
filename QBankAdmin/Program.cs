var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();


var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapDefaultControllerRoute();

app.Run();
