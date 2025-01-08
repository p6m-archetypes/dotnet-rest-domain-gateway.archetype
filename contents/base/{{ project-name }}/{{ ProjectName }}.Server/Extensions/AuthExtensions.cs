using Microsoft.AspNetCore.Authorization;

namespace {{ ProjectName }}.Server.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtBearerAuth(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Security:OAuth2:Authority"];
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = configuration.GetValue("Security:OAuth2:ValidateIssuer", false),
                    ValidIssuer = configuration.GetValue("Security:OAuth2:ValidIssuer", ""),
                    ValidateAudience = configuration.GetValue("Security:OAuth2:ValidateAudience", false),
                    ValidAudience = configuration.GetValue("Security:OAuth2:ValidAudience", ""),
                    ValidateLifetime = configuration.GetValue("Security:OAuth2:ValidateLifetime", true),
                    ValidateIssuerSigningKey = configuration.GetValue("Security:OAuth2:ValidateIssuerSigningKey", false)
                };
            });

        services.AddAuthorization(options =>
        {
            //Deny all unauthenticated requests
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}