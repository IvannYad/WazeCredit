﻿using Microsoft.AspNetCore.Mvc;
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
        public HomeViewModel HomeViewModel { get; set; }
        public HomeController(IMarketForecaster marketForecaster)
        {
            HomeViewModel = new HomeViewModel();
            _marketForecaster = marketForecaster;
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

        public IActionResult AllConfigSettings(
            [FromServices] IOptions<StripeSettings> stripeOptions,
            [FromServices] IOptions<WazeForecastSettings> wazeForecastOptions,
            [FromServices] IOptions<SendGridSettings> sendGridOptions,
            [FromServices] IOptions<TwilioSettings> twilioOptions
            )
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: " + wazeForecastOptions.Value.ForecastTrackerEnabled);
            messages.Add($"Stripe config - Publishable Key: " + stripeOptions.Value.PublishableKey);
            messages.Add($"Stripe config - Secret Key: " + stripeOptions.Value.SecretKey);
            messages.Add($"SendGrid config - SendGrid Key: " + sendGridOptions.Value.SendGridKey);
            messages.Add($"Twilio config - AccountSid: " + twilioOptions.Value.AccountSid);
            messages.Add($"Twilio config - Auth Token: " + twilioOptions.Value.AuthToken);
            messages.Add($"Twilio config - Phone Number: " + twilioOptions.Value.PhoneNumber);

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