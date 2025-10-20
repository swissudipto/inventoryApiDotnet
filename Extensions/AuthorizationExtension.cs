namespace inventoryApiDotnet.AuthorizationExtension;

public static class AuthorizationExtension
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection service)
    {
        service.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
            policy.RequireRole("Admin"));

            options.AddPolicy("StaffOrAdmin", policy =>
            policy.RequireRole("Admin", "Staff"));
        });

        return service;
    }
}