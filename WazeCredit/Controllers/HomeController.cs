using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Services;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMarketForecaster _marketForecaster;
        private readonly StripeSettings _stripeOptions;
        private readonly WazeForecastSettings _wazeForecastOptions;
        private readonly SendGridSettings _sendGridOptions;
        private readonly TwilioSettings _twilioOptions;
        public HomeViewModel HomeViewModel { get; set; }
        public HomeController(IMarketForecaster marketForecaster,
            IOptions<StripeSettings> stripeOptions,
            IOptions<WazeForecastSettings> wazeForecastOptions,
            IOptions<SendGridSettings> sendGridOptions,
            IOptions<TwilioSettings> twilioOptions)
        {
            HomeViewModel = new HomeViewModel();
            _marketForecaster = marketForecaster;
            _stripeOptions = stripeOptions.Value;
            _wazeForecastOptions = wazeForecastOptions.Value;
            _sendGridOptions = sendGridOptions.Value;
            _twilioOptions = twilioOptions.Value;

        }
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            MarketResult currentMarket = _marketForecaster.GetMarketResult();

            switch (currentMarket.MarketCondition)
            {
                case MarketCondition.StableDown:
                    homeViewModel.MarketForecast = "Market shows signs to go down in a stable state! It is a not a good sign to apply for credit applications! But extra credit is always piece of mind if you have handy when you need it.";
                    break;
                case MarketCondition.StableUp:
                    homeViewModel.MarketForecast = "Market shows signs to go up in a stable state! It is a great sign to apply for credit applications!";
                    break;
                case MarketCondition.Volatile:
                    homeViewModel.MarketForecast = "Market shows signs of volatility. In uncertain times, it is good to have credit handy if you need extra funds!";
                    break;
                default:
                    homeViewModel.MarketForecast = "Apply for a credit card using our application!";
                    break;
            }

            return View(homeViewModel);
        }

        public IActionResult AllConfigSettings()
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: " + _wazeForecastOptions.ForecastTrackerEnabled);
            messages.Add($"Stripe config - Publishable Key: " + _stripeOptions.PublishableKey);
            messages.Add($"Stripe config - Secret Key: " + _stripeOptions.SecretKey);
            messages.Add($"SendGrid config - SendGrid Key: " + _sendGridOptions.SendGridKey);
            messages.Add($"Twilio config - AccountSid: " + _twilioOptions.AccountSid);
            messages.Add($"Twilio config - Auth Token: " + _twilioOptions.AuthToken);
            messages.Add($"Twilio config - Phone Number: " + _twilioOptions.PhoneNumber);

            return View(messages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}