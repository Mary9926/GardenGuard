using GardenGuardFrontend.Server;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

builder.Configuration
    .AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<AzureStorageSettings>(
    builder.Configuration.GetSection(nameof(AzureStorageSettings)));
builder.Services.AddSingleton<SensorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
