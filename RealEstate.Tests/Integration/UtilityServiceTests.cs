using FluentAssertions;
using RealEstate.Business.Utilities;
using RealEstate.Tests.Helpers;
using System.Diagnostics;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace RealEstate.Tests.Integration
{
    public class UtilityServiceTests : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly WireMockServer _mockServer;
        private readonly UtilityService _utilityService;
        public UtilityServiceTests()
        {
            _mockServer = WireMockServer.Start();
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_mockServer.Urls[0])
            };
            _utilityService = new UtilityService(_httpClient);
        }

        public void Dispose()
        {
            _mockServer.Stop();
            _httpClient.Dispose();
        }

        //Positive Case
        [Test]
        public async Task GivenValidResident_WhenGetResidentUtilityBalanceByIdAsyncIsInvoked_ThenValidResidentUtilityBalanceIsReturned()
        {
            //Arrange
            var customerId = 23;
            var faker = new ResidentUtilityDtoFaker(customerId);
            var residentBalance = faker.Generate();
            _mockServer.Given(Request.Create().UsingGet().WithPath($"/balances/v2/{customerId}"))
                                 .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBodyAsJson(residentBalance));

            //Act
            var balanceResponse = await _utilityService.GetResidentUtilityBalanceByIdAsync(customerId);

            //Assert
            balanceResponse.Should().NotBeNull();
            balanceResponse.ElectricityBalance.Should().Be(residentBalance.ElectricityBalance);
            balanceResponse.TrashBalance.Should().Be(residentBalance.TrashBalance);
            balanceResponse.WaterBalance.Should().Be(residentBalance.WaterBalance);
        }

        //Negative Case
        [Test]
        public async Task GivenInValidResident_WhenGetResidentUtilityBalanceByIdAsyncIsInvoked_ThenNullIsReturned()
        {
            //Arrange
            var customerId = 42;
            _mockServer.Given(Request.Create().UsingGet().WithPath($"/balances/v2/{customerId}"))
                                 .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.NotFound));

            //Act
            var balanceResponse = await _utilityService.GetResidentUtilityBalanceByIdAsync(customerId);

            //Assert
            balanceResponse.Should().BeNull();
        }

        [Test]
        public async Task GivenValidResident_WhenGetResidentUtilityBalanceByIdAsyncIsInvoked_ShouldHandleDelayedResponses()
        {
            //Arrange
            var customerId = 23;
            var faker = new ResidentUtilityDtoFaker(customerId);
            var residentBalance = faker.Generate();
            _mockServer.Given(Request.Create().UsingGet().WithPath($"/balances/v2/{customerId}"))
                                 .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK)
                                 .WithBodyAsJson(residentBalance).WithDelay(TimeSpan.FromMilliseconds(200)));

            //Act
            var watch = new Stopwatch();
            watch.Start();
            var balanceResponse = await _utilityService.GetResidentUtilityBalanceByIdAsync(customerId);
            watch.Stop();

            //Assert
            balanceResponse.Should().NotBeNull();
            watch.ElapsedMilliseconds.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task GivenValidResident_WhenGetResidentUtilityBalanceByIdAsyncIsInvoked_ShouldHandleFaults()
        {
            //Arrange
            var customerId = 23;
            var faker = new ResidentUtilityDtoFaker(customerId);
            var residentBalance = faker.Generate();
            _mockServer.Given(Request.Create().UsingGet().WithPath($"/balances/v2/{customerId}"))
                                 .RespondWith(Response.Create().WithFault(FaultType.MALFORMED_RESPONSE_CHUNK));

            //Act
            var balanceResponse = await _utilityService.GetResidentUtilityBalanceByIdAsync(customerId);

            //Assert
            balanceResponse.Should().BeNull();
        }

    }
}
