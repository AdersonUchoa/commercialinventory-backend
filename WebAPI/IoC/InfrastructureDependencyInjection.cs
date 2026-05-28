using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.IoC
{
    public static class InfrastructureDependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDatabaseContexts(services, configuration);
            AddRepositories(services);
        }

        private static void AddDatabaseContexts(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

            services.AddDbContext<CommercialInventoryDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
        }
    }
}
