using Application.Interfaces;
using Application.Mappers.Profiles;
using Application.Services;

namespace WebAPI.IoC
{
    public static class ApplicationDependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddServices(services);
            AddAutoMapper(services);
        }
        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProdutoService, ProdutoService>();
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CategoriaProfile).Assembly));
        }

    }
}
