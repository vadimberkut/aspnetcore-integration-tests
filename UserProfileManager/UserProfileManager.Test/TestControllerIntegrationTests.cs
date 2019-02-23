using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UserProfileManager.Entity.Dto;
using Xunit;

namespace UserProfileManager.Test
{
    public class TestControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public TestControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ShouldReturnEmptyResult()
        {
            var httpResponse = await _client.GetAsync("/api/test/empty");
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            JsonResponseDto dto = JsonConvert.DeserializeObject<JsonResponseDto>(stringResponse);

            JsonResponseDto expectedDto = new JsonResponseDto()
            {
                Data = "",
                Status = 200,
                Message = null
            };
            
            Assert.NotNull(dto);
            Assert.Equal(expectedDto, dto);
        }
    }
}