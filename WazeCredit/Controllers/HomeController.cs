using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Diagnostics;
using WazeCredit.Data;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Services;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMarketForecaster _marketForecaster;
        private readonly ICreditValidator _creditValidator;
        private readonly IOptions<StripeSettings> _stripeOptions;
        private readonly ApplicationDbContext _context;

        // BindProperty works properly with public properties.
        [BindProperty]
        public CreditApplication CreditModel { get; set; }

        public HomeViewModel HomeViewModel { get; set; }
        public HomeController(IMarketForecaster marketForecaster
            , IOptions<StripeSettings> stripeOptions
            , ICreditValidator creditValidator
            , ApplicationDbContext context)
        {
            HomeViewModel = new HomeViewModel();
            _marketForecaster = marketForecaster;
            _stripeOptions = stripeOptions;
            _creditValidator = creditValidator;
            _context = context;
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
            [FromServices] IOptions<WazeForecastSettings> wazeForecastOptions,
            [FromServices] IOptions<SendGridSettings> sendGridOptions,
            [FromServices] IOptions<TwilioSettings> twilioOptions
            )
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: " + wazeForecastOptions.Value.ForecastTrackerEnabled);
            messages.Add($"Stripe config - Publishable Key: " + _stripeOptions.Value.PublishableKey);
            messages.Add($"Stripe config - Secret Key: " + _stripeOptions.Value.SecretKey);
            messages.Add($"SendGrid config - SendGrid Key: " + sendGridOptions.Value.SendGridKey);
            messages.Add($"Twilio config - AccountSid: " + twilioOptions.Value.AccountSid);
            messages.Add($"Twilio config - Auth Token: " + twilioOptions.Value.AuthToken);
            messages.Add($"Twilio config - Phone Number: " + twilioOptions.Value.PhoneNumber);

            return View(messages);
        }

        public IActionResult CreditApplication()
        {
            CreditModel = new CreditApplication();
            return View(CreditModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("CreditApplication")]
        public async Task<IActionResult> CreditApplicationPOST([FromServices] Func<CreditApprovedEnum, ICreditApproved> _creditService)
        {
            if (ModelState.IsValid)
            {
                var (validationPassed, errorMessages) = await _creditValidator.PassAllValidations(CreditModel);
                CreditResult creditResult = new CreditResult()
                {
                    ErrorList = errorMessages,
                    CreditID = 0,
                    Success = validationPassed,
                };

                if (validationPassed)
                {
                    CreditModel.CreditApproved = _creditService(CreditModel.Salary > 50000 ? CreditApprovedEnum.High : CreditApprovedEnum.Low)
                        .GetCreditApproved(CreditModel);
                    _context.CreditApplications.Add(CreditModel);
                    _context.SaveChanges();
                    creditResult.CreditID = CreditModel.Id;
                    creditResult.CreditApproved = CreditModel.CreditApproved;
                    return RedirectToAction(nameof(CreditResult), creditResult);
                }
                else
                {
                    return RedirectToAction(nameof(CreditResult), creditResult);
                }
            }

            return View(CreditModel);
        }

        public IActionResult CreditResult(CreditResult creditResult)
        {
            return View(creditResult);
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