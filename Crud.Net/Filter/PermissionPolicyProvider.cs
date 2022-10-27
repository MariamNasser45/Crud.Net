using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Crud.Net.Filter

// action of IAuthorizationPolicyProvider
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        // implementation of IAuthorizationPolicyProvider methods 

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }  //property will initialize in constructor

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        // define implementation we need in next method
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyname)
        {
            // cheack on the recieved name is permission or not

            if (policyname.StartsWith("Permisions", StringComparison.OrdinalIgnoreCase)) //If recieved policyname start with word "permiossion" apply next code
            {                                                                            //StringComparison.OrdinalIgnoreCase : to ignore letter case

                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyname));
                return Task.FromResult(policy.Build());
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyname);
        }

        // now go to program.cs  to regester this services
    }
}
