using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AppDevicesMedical.Authorization
{
    public class PermisoPolicyProvider : IAuthorizationPolicyProvider
    {
        const string PREFIX = "Permiso:";
        private readonly DefaultAuthorizationPolicyProvider _fallback;

        public PermisoPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallback = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var permiso = policyName.Substring(PREFIX.Length);
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermisoRequerido(permiso))
                    .Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            return _fallback.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallback.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallback.GetFallbackPolicyAsync();
    }

}
