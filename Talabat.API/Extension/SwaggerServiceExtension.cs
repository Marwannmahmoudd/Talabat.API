namespace Talabat.API.Extension
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }


        public static WebApplication UseSwaggerExt(this WebApplication application)
        {
            application.UseSwagger();
            application.UseSwaggerUI();

            return application;
        }

    }
}
