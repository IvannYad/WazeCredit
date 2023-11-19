using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Configuration;
using WazeCredit.Data;
using WazeCredit.Middleware;
using WazeCredit.Services;
using WazeCredit.Services.LifetimeExample;
using WazeCredit.Utility.AppSettingsClasses;
using WazeCredit.Utility.DI_Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IMarketForecaster, MarketForecasterV2>();
builder.Services.TryAddTransient<IMarketForecaster, MarketForecaster>();
builder.Services.AddAppSettingsConfig(builder.Configuration);
builder.Services.AddTransient<TransientService>();
builder.Services.AddScoped<ScopedService>();
builder.Services.AddSingleton<SingletonService>();

//builder.Services.AddScoped<IValidationChecker, AddressValidationChecker>();
//builder.Services.AddScoped<IValidationChecker, CreditValidationChecker>();
//builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>());
//builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>());
builder.Services.TryAddEnumerable(new[] {
    ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>(),
    ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>(),
});

builder.Services.AddScoped<ICreditValidator, CreditValidator>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<CustomMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
