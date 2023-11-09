using System.Configuration;
using WazeCredit.Services;
using WazeCredit.Utility.AppSettingsClasses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IMarketForecaster, MarketForecasterV2>();
builder.Services.Configure<WazeForecastSettings>(builder.Configuration.GetSection("WazeForecast"));
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
