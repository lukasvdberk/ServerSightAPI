using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ServerSightAPI.DTO.Server;
using ServerSightAPI.DTO.User;
using ServerSightAPI.Tests.Integration.Integration.User;
using Xunit;

namespace ServerSightAPI.Tests.Integration.Integration.Server
{
    public class Server : IntegrationTest
    {
        // TO test
        //  create server
        //    - with good information
        //    - with false information
        //  image upload
        //    - image gets saved
        //    - overriding
        //  get server and servers
        //    - with right user
        //    - unauthorized
        //    - correctly
        //    - test filters
        //      - correct
        //      - false
        //  delete server
        //    - unauthorized
        //    - correctly
        
        [Fact]
        public async Task CreateServerOk()
        {
            await AuthenticateAsync();
            var response = await TestClient.PostAsJsonAsync("/api/servers", new CreateServerDto()
            {
                Name = "Test server",
                Description = "# My description",
                PowerStatus = true
            });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var authResponse = await response.Content.ReadAsAsync<AuthResponse>();

            authResponse.Token.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task CreateServerWithFalseInformation()
        {
            await AuthenticateAsync();
            
            // name is to long
            var reponseWithToLongName = await TestClient.PostAsJsonAsync("/api/servers", new CreateServerDto()
            {
                Name = "Test server alskdfjljasdlkfjkal sjdklfajsdfjlj",
                Description = "# My description",
                PowerStatus = true
            });

            reponseWithToLongName.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            var responseWithEmptyDescription = await TestClient.PostAsJsonAsync("/api/servers", new CreateServerDto()
            {
                Name = "Test server",
                Description = null,
                PowerStatus = true
            });

            responseWithEmptyDescription.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}