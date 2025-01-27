using Microsoft.Extensions.DependencyInjection;

namespace CantineKata.Application
{
    public static class DI
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(DI).Assembly));

            return services;
        }
    }
}
