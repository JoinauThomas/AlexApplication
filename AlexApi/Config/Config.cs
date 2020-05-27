using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlexApi.Config
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    //Definition du type de client
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,

                    //Definition de la SecretKey
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    //Contexte auquel le client peut acceder
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OfflineAccess, "api1"}
                }
            };
        }

        // creation d'un user tests en attendant qu'on cree la DB
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "test",
                    Password = "password"
                }
            };
        }
    }
}
