using Microsoft.OpenApi.Models;

namespace {{ ProjectName }}.Server.Extensions;

internal static class AuthSwaggerExtensions
{
    internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo{ Title = "{{ project-title }}", Version = "v1" });
            options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(configuration["Security:OAuth2:AuthorizationUrl"] ?? ""),
                        TokenUrl = new Uri(configuration["Security:OAuth2:TokenUrl"] ?? "")
                    }
                }
            });
    
            OpenApiSecurityScheme securityScheme = new()
            {
                Reference = new OpenApiReference
                {
                    Id = "OAuth2",
                    Type = ReferenceType.SecurityScheme,
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer",
            };

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() },
            });
        });

        return services;
    }

    internal static IApplicationBuilder UseSwaggerUIWithAuth(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.yaml", "{{ project-title }} V1");
            options.RoutePrefix = string.Empty;

            options.OAuthClientId(configuration["Security:OAuth2:ClientId"]);
            options.OAuthUsePkce();
            options.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
            {
                {"scope", "openid profile email offline_access"}
            });
        });

        return app;
    }
}