using System.IO;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServerSightAPI.Configurations.Services
{
    public static class FirebaseInitialization
    {
        public static void ConfigureFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            var fireBaseSection = configuration.GetSection("Firebase");
            var credential = GoogleCredential.FromJson(fireBaseSection.Value);

            FirebaseApp.Create(new AppOptions()  
            {
                Credential = credential  
            });  
        }
    }
}