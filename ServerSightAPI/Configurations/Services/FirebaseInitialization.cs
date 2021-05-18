using System;
using System.Collections.Generic;
using System.IO;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ServerSightAPI.Configurations.Services
{
    public static class FirebaseInitialization
    {
        public static void ConfigureFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                Dictionary<string, string> firebaseConfigs = new Dictionary<string, string>();
            
                configuration.GetSection("Firebase").Bind(firebaseConfigs);
                var credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(firebaseConfigs));

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = credential
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine("Firebase app already configured");
            }
        }
    }
}