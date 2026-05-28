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
            //TODO services.AddScoped para Categoria e Produto
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            //TODO: Para Profile caso necessario das entidades Categoria e Produto
            //services.AddAutoMapper(cfg => cfg.AddMaps(typeof(PessoaProfile).Assembly));
        }

    }
}
