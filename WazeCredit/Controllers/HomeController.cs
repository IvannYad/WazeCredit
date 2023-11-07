using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Services;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            MarketForecaster marketForecaster = new MarketForecaster();
            MarketResult currentMarket = marketForecaster.GetMarketResult();

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