﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[] { 
                new ApiResource("api1", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[] { 
                new Client
                {
                    ClientId="client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes= new List<string>{"api1"}
                },
                new Client
                {
                    ClientId="ro.client",
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,

                    ClientSecrets=new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes= new List<string>{"api1"}
                }

            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="alice",
                    Password="password"
                },
                new TestUser
                {
                    SubjectId="2",
                    Username="bob",
                    Password="password"
                }
            };
        }
    }
}