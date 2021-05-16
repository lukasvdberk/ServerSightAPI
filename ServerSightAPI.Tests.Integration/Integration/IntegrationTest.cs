using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServerSightAPI.Configurations;
using ServerSightAPI.DTO.User;
using ServerSightAPI.Tests.Integration.Integration.User;
using Xunit.Sdk;

namespace ServerSightAPI.Tests.Integration.Integration
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        protected ServiceCollection ServiceProvider;
        protected WebApplicationFactory<Startup> Factory;
        protected UserManager<Models.User> UserManager;
        
        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));
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
            // TODO refactor to separate class.
            var userDto = new UserDTO
            {
                Email = "test@integration.com",
                Password = "knMNU8X7@1780!"
            };

            // await DeletePreviousUser(userDto);

            var registerResponse = await TestClient.PostAsJsonAsync("/api/users/register", userDto);

            if (!registerResponse.IsSuccessStatusCode)
            {
                // if not 500 then the user just exists and failed for that reason not because the endpoint is broken
                var registrationFailed = registerResponse.StatusCode == HttpStatusCode.InternalServerError;
                if (registrationFailed)
                {
                    // TODO check exact error not internal server error
                    throw new Exception("Failed to register user");   
                }
            }
            
            var loginResponse = await TestClient.PostAsJsonAsync("/api/users/login", userDto);
            var authResponse = await loginResponse.Content.ReadAsAsync<AuthResponse>();

            return authResponse.Token;
        }

        public UserManager<Models.User> GetUserManager()
        {
            try
            {
                return UserManager;
            }
            catch (NullException exception)
            {
                throw new Exception("Could not get instance of user manager of database");
            }
        }
    }
}