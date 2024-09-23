using AuthFlow;
using AuthFlow.Data;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(builder =>
    {
        var configuration = builder.Build();
        var token = new DefaultAzureCredential();
        var appConfigUrl = configuration["app_config_url"] ?? string.Empty;
        
        builder.AddAzureAppConfiguration(config =>
        {
            config.Connect(new Uri(appConfigUrl), token);
            config.ConfigureKeyVault(kv => kv.SetCredential(token));
        });
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        
        services.RegisterServices(new DatabaseOption
        {
            ConnectionString = configuration["database_connection_string"] ?? string.Empty,
        },tokenConfig =>
        {
            tokenConfig.TokenEndpoint = configuration["token_endpoint_in_app_configuration"] ?? string.Empty;
            tokenConfig.SignInUpPolicy = configuration["sign_in_up_policy_in_app_configuration"] ?? string.Empty;
            tokenConfig.UpdatePolicy = configuration["update_policy_in_app_configuration"] ?? string.Empty;
            tokenConfig.ClientId = configuration["client_id_in_app_configuration"] ?? string.Empty;
            tokenConfig.ClientSecret = configuration["client_secret_in_app_configuration"] ?? string.Empty;
            tokenConfig.Scope = configuration["scope_in_app_configuration"] ?? string.Empty;
            tokenConfig.GrantType = configuration["grant_type_in_app_configuration"] ?? string.Empty;
        }, authOpt =>
        {
            authOpt.PostRegisterRedirectUri = configuration["post_register_redirect_uri_in_app_configuration"] ?? string.Empty;
            authOpt.PostUpdateRedirectUri = configuration["post_update_redirect_uri_in_app_configuration"] ?? string.Empty;
            authOpt.UnauthorizedRedirectUri = configuration["unauthorized_redirect_uri_in_app_configuration"] ?? string.Empty;
        });

    })
    .Build();

host.Run();