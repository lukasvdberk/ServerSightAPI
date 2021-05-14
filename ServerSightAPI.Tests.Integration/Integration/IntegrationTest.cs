using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServerSightAPI.Configurations;
using ServerSightAPI.DTO.User;

namespace ServerSightAPI.Tests.Integration.Integration
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>();
                // For in memory database!
                // .WithWebHostBuilder(builder =>
                // {
                //     builder.ConfigureServices(services =>
                //     {
                //         services.RemoveAll(typeof(DatabaseContext));
                //         services.AddDbContext<DatabaseContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                //     });
                // });
            
            TestClient = appFactory.CreateClient();
        }
        
        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await RegisterAndGetJwt());
        }
        
        private async Task<string> RegisterAndGetJwt()
        {
            var response = await TestClient.PostAsJsonAsync("/api/users/login", new UserDTO
            {
                Email = "test@integration.com",
                Password = "SomePass1234!"
            });

            var registrationResponse = await response.Content.ReadFromJsonAsync<Dictionary<String, String>>();

            string token = "";
            if (registrationResponse != null) registrationResponse.TryGetValue("Token", out token);

            return token;
        }
    }
}