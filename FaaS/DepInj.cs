using AuthFlow.Data;
using AuthFlow.Data.Database;
using AuthFlow.Service;
using AuthFlow.Service.Interfaces;
using AuthFlow.Service.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthFlow
{
    public static class DepInj
    {
        public static void RegisterServices(
            this IServiceCollection services, 
            DatabaseOption dbOption, 
            Action<TokenServiceOptions> tokenServiceOptions, 
            Action<AuthServiceOptions> authServiceOptions)
        {
            services.AddDatabaseContext<AuthFlowDbContext>(dbOption);
            services.ConfigureServiceOptions<TokenServiceOptions>((_, opt) => tokenServiceOptions(opt));
            services.ConfigureServiceOptions<AuthServiceOptions>((_, opt) => authServiceOptions(opt));
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();
        }
        
        private static void ConfigureServiceOptions<TOptions>(
            this IServiceCollection services,
            Action<IServiceProvider, TOptions> configure)
            where TOptions : class
        {
            services
                .AddOptions<TOptions>()
                .Configure<IServiceProvider>((options, resolver) => configure(resolver, options));
        }
        
        private static void AddDatabaseContext<TContext>(
            this IServiceCollection services, DatabaseOption dbConnectionsOptions)
            where TContext : DbContext
        {
            
            services.AddDbContext<TContext>(
                options =>
                {
                    if (!options.IsConfigured)
                    {
                        options.UseSqlServer(dbConnectionsOptions.ConnectionString, opt =>
                        {
                            opt.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        });
                    }
                });
        }
    }
}