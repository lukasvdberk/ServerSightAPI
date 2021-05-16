using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Google.Apis.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ServerSightAPI.Configurations;
using ServerSightAPI.DTO.User;
using ServerSightAPI.Tests.Integration.Integration.User;

namespace ServerSightAPI.Tests.Integration.Integration
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                 typeof(DbContextOptions<DatabaseContext>));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }
                        services.AddDbContext<DatabaseContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForIntegrationTesting");
                        });
                        
                        var sp = services.BuildServiceProvider();
                        using var scope = sp.CreateScope();
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<DatabaseContext>();

                        db.Database.EnsureCreated();
                    });
                });
            
            TestClient = appFactory.CreateClient();
        }
        
        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await RegisterAndGetJwt());
        }
        
        private async Task<string> RegisterAndGetJwt()
        {
            // TODO refactor to seperate class.
            var userDto = new UserDTO
            {
                Email = "test@integration.com",
                Password = "knMNU8X7@1780!"
            };
            
            var registerResponse = await TestClient.PostAsJsonAsync("/api/users/register", userDto);

            if (!registerResponse.IsSuccessStatusCode)
            {
                throw new Exception("Failed to register user");
            }
            
            var loginResponse = await TestClient.PostAsJsonAsync("/api/users/login", userDto);
            var authResponse = await loginResponse.Content.ReadAsAsync<AuthResponse>();

            return authResponse.Token;
        }
    }
}