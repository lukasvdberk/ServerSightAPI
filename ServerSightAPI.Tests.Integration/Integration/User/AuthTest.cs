using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ServerSightAPI.DTO.User;
using Xunit;

namespace ServerSightAPI.Tests.Integration.Integration.User
{   
    public class AuthResponse
    {
        public string Token { get; set; }
    }
    
    public class AuthTest : IntegrationTest
    {
        [Fact]
        public async Task LoginExistingUser()
        {
            // create default user with the same credentials as below.
            await AuthenticateAsync();
            var response = await TestClient.PostAsJsonAsync("/api/users/login", new UserDTO()
            {
                Email = "test@integration.com",
                Password = "knMNU8X7@1780!"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            var authResponse = await response.Content.ReadAsAsync<AuthResponse>();

            authResponse.Token.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task LoginExistingUserWrongCredentials()
        {
            var registrationResponse = await TestClient.PostAsJsonAsync("/api/users/register", new UserDTO()
            {
                Email = "testtest@integration.com",
                Password = "knMNU8X7@17f80!"
            });

            registrationResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var response = await TestClient.PostAsJsonAsync("/api/users/login", new UserDTO()
            {
                Email = "testtest@integration.com",
                Password = "789342789234789f"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var authResponse = await response.Content.ReadAsAsync<AuthResponse>();

            authResponse.Token.Should().NotBeEmpty();
        }

        [Fact]
        public async Task RegisterUser()
        {
            var response = await TestClient.PostAsJsonAsync("/api/users/register", new UserDTO()
            {
                Email = "test1231231231231@integration.com",
                Password = "knMNU8X7@17f80!"
            });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task RegisterUserWithNoWrongPasswordRequirements()
        {
            var response = await TestClient.PostAsJsonAsync("/api/users/register", new UserDTO()
            {
                Email = "test1231231231231@integration.com",
                Password = "Password"
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task RegisterUserWithAlreadyExistingUser()
        {
            var succesFullUserRegristration = await TestClient.PostAsJsonAsync("/api/users/register", new UserDTO()
            {
                Email = "test1@integration.com",
                Password = "knMNU8X7@17f80!"
            });

            succesFullUserRegristration.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var repeatedUserRegristration = await TestClient.PostAsJsonAsync("/api/users/register", new UserDTO()
            {
                Email = "test1@integration.com",
                Password = "knMNU8X7@17f80!"
            });

            repeatedUserRegristration.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}