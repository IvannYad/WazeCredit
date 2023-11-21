using WazeCredit.Models;
using WazeCredit.Services.LifetimeExample;
using WazeCredit.Services;
using WazeCredit.Utility.AppSettingsClasses;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WazeCredit.Data.Repository.IRepository;
using WazeCredit.Data.Repository;

namespace WazeCredit.Utility.DI_Config
{
    public static class ConfigureDIServices
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllersWithViews();
            services.AddTransient<IMarketForecaster, MarketForecasterV2>();
            services.TryAddTransient<IMarketForecaster, MarketForecaster>();
            services.AddTransient<TransientService>();
            services.AddScoped<ScopedService>();
            services.AddSingleton<SingletonService>();

            //builder.Services.AddScoped<IValidationChecker, AddressValidationChecker>();
            //builder.Services.AddScoped<IValidationChecker, CreditValidationChecker>();
            //builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>());
            //builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>());
            services.TryAddEnumerable(new[] {
                ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>(),
                ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>(),
            });

            services.AddScoped<ICreditValidator, CreditValidator>();

            services.AddScoped<CreditApprovedHigh>();
            services.AddScoped<CreditApprovedLow>();
            services.AddScoped<Func<CreditApprovedEnum, ICreditApproved>>(ServiceProvider => range =>
            {
                switch (range)
                {
                    case CreditApprovedEnum.Low:
                        return ServiceProvider!.GetService<CreditApprovedLow>()!;
                    case CreditApprovedEnum.High:
                        return ServiceProvider!.GetService<CreditApprovedHigh>()!;
                    default:
                        return ServiceProvider!.GetService<CreditApprovedLow>()!;
                }
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
