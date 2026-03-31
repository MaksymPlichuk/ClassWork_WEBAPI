using ClassWork_WEBAPI.BLL.Services;
using ClassWork_WEBAPI.DAL.Repositories;
using System.Runtime.CompilerServices;

namespace ClassWork_WEBAPI.API.Infrastracture
{
    public static class ServiceBuilderExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<AuthorService>();
            services.AddScoped<BookService>();
            services.AddScoped<ImageService>();
            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();
            services.AddScoped<EmailService>();
            return services; 
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<AuthorRepository>();
            services.AddScoped<GenreRepository>();
            services.AddScoped<BookRepository>();
            return services;
        }
    }
}
