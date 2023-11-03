using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// This is middleware code for caching stylesheets (so it can render on time and not look ugly) on the development server, it's for testing during development only!
app.MapWhen(context => context.Request.Path.StartsWithSegments("/css/app.css"), appBuilder =>
        {
            appBuilder.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Cache-Control", "max-age=31536000"); // 1 year
                context.Response.Headers.Add("Expires", DateTime.UtcNow.AddYears(1).ToString("R"));  
                await next.Invoke();
            });
            appBuilder.UseStaticFiles(); // Serve the CSS file
        });

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
