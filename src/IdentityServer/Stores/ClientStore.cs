using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ClientStore : IClientStore
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientStore(
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            // return null;

            var client = new Client
            {
                ClientId = "wl_client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                    {
                        new Secret("wl_secret".Sha256())
                    },

                // scopes that client has access to
                AllowedScopes = { "email_access", "promotion_access" },

            };


            //var permission = _httpContextAccessor.HttpContext.Request.Headers["permission"].ToString();
            //var permissions = permission.Split(";");

            //foreach (var perm in permissions)
            //{
            //    client.Claims.Add(new ClientClaim("permission", perm));
            //}
            
            //client.ClientClaimsPrefix = "";

            return await Task.FromResult(client);
        }
    }
}
