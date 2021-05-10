// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),

                // custom identity resource with some associated claims
                new IdentityResource("custom.profile",
                    userClaims: new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, "location", JwtClaimTypes.Address })
                };

        public static IEnumerable<ApiScope> GetScopes()
        {
            return new List<ApiScope>
            {
                //new ApiScope("api1", "My API"),
                new ApiScope("promotion.access", "Access to promotion API"),
                new ApiScope("email.access", "Access to email API"),

                new ApiScope("api.access", "Access to APIs")

                //new ApiScope(name: "read",   displayName: "Read your data."),
                //new ApiScope(name: "write",  displayName: "Write your data."),
                //new ApiScope(name: "delete", displayName: "Delete your data.")
            };
        }


        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("promotion", "Promotion API")
                {
                    Scopes = { "api.access" }
                },

                new ApiResource("email", "Email API")
                {
                    Scopes = { "api.access" }
                }

            };
        }

        //public static IEnumerable<ApiScope> ApiScopes =>
        //    new List<ApiScope>
        //    {
        //        new ApiScope("api1", "My API"),
        //        new ApiScope("api_promotion", "Promotion")
        //    };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api.access" },
                }
            };
    }
}