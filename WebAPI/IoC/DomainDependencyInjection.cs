using Domain.Interfaces.SeedWorks;
using Infrastructure.SeedWorks;

namespace WebAPI.IoC
{
    public static class DomainDependencyInjection
    {
        public static void AddDomain(this IServiceCollection services)
        {
            AddSeedWorks(services);
        }

        private static void AddSeedWorks(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
