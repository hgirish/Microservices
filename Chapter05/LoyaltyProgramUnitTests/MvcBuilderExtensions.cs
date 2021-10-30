using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LoyaltyProgramUnitTests
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddControllersByType(this IServiceCollection services,
            params Type[] controllerTypes) =>
            services.AddControllers()
            .ConfigureApplicationPartManager(mgr =>
            {
                mgr.FeatureProviders.Remove(mgr.FeatureProviders.First(f => f is ControllerFeatureProvider));
                mgr.FeatureProviders.Add(new FixedControllerProvider(controllerTypes));
            }
            );
    }
}
