using CryptoPortfolioApp.Components;
using CryptoPortfolioApp.Components.Interfaces;
using CryptoPortfolioApp.Components.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/portfolio.log", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<CoinloreService>(client =>
{
    client.BaseAddress = new Uri("https://api.coinlore.net/");
});

builder.Services.AddScoped<IPortfolioCalculatorService, PortfolioCalculatorService>();
builder.Services.AddScoped<ICoinloreService, CoinloreService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
