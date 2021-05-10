using IdentityServer4.Models;
using IdentityServer4.Stores;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ResourceStore : IResourceStore
    {

        private readonly IRedisDatabase _redisDatabase;

        public ResourceStore(
            IRedisDatabase redisDatabase
            )
        {
            _redisDatabase = redisDatabase;
        }


        /*
         resources
         [{"name":"promotion","displayName":"Promotion API","scopes":["promotion_access","promotion_read"]},{"name":"email","displayName":"Email API","scopes":["email_access","email_read"]}]
         
         scopes
         ["email_access","promotion_access"]
         */

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            return Task.FromResult(new List<ApiResource>().AsEnumerable());
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            // return await Task.FromResult(new List<ApiResource>().AsEnumerable());

            var access = await _redisDatabase.GetAsync<IEnumerable<Access>>("resources");

            var apiResouces = access.Where(x => x.Scopes.Any(y => scopeNames.Contains(y))).Select(x => new ApiResource
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                Scopes = x.Scopes
            });

            //var apiResouces = access.Where(x => scopeNames.Contains(x.Scope)).Select(x => new ApiResource
            //{
            //    Name = x.Name,
            //    DisplayName = x.DisplayName,
            //    Scopes = { x.Scope }
            //});

            return apiResouces;

            //var apiResouces = new List<ApiResource>();

            //foreach (var item in access)
            //{
            //    apiResouces.Add(new ApiResource
            //    {
            //        Name = item.Value.Name,
            //        DisplayName = item.Value.DisplayName,
            //        Scopes = { item.Key }
            //    });
            //}


            // =======================================================================================================
            // var access = await _redisDatabase.GetAsync<IEnumerable<Access>>(scopeNames.First());
            //var apiResouces = new List<ApiResource>
            //{
            //    new ApiResource("promotion", "Promotion API")
            //    {
            //        Scopes = { "api.access" }
            //    },

            //    new ApiResource("email", "Email API")
            //    {
            //        Scopes = { "api.access" }
            //    }
            //};

            //  return apiResouces;
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            // return await Task.FromResult(new List<ApiScope>().AsEnumerable());

            var allScopes = await _redisDatabase.GetAsync<string[]>("scopes");

            var apiScopes = allScopes.Where(x => scopeNames.Contains(x)).Select(x => new ApiScope { Name = x });

            return apiScopes;

            // var access = await _redisDatabase.GetAllAsync<Access>(scopeNames);
            //var apiScopes = new List<ApiScope>();

            //foreach (var item in access)
            //{
            //    apiScopes.Add(new ApiScope
            //    {
            //        Name = item.Key,
            //        DisplayName = item.Value.DisplayName
            //    });
            //}

            // ===========================================================================
            //var apiScopes = new List<ApiScope>
            //{
            //    new ApiScope("api.access", "Access to APIs")
            //};

            // return apiScopes;
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var identityResource = new IdentityResource[]
            {
                 new IdentityResources.OpenId()
            };

            return await Task.FromResult(identityResource);
        }

        public Task<Resources> GetAllResourcesAsync()
        {

            return Task.FromResult(new Resources());

            //var resources = new Resources();

            //var access = await _redisDatabase.GetAsync<IEnumerable<Access>>("identity.access");

            //var apiResources = access.Select(x => new ApiResource
            //{
            //    Name = x.Name,
            //    DisplayName = x.DisplayName,
            //    Scopes = x.Scopes
            //});

            //var apiScopes = access.SelectMany(x => x.Scopes).Select(x => new ApiScope
            //{
            //    Name = x
            //});

            //resources.ApiResources = apiResources.ToArray();
            //resources.ApiScopes = apiScopes.ToArray();

            //return resources;
        }
    }
}
