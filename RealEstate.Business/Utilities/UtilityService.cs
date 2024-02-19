using System.Net;
using System.Text.Json;

namespace RealEstate.Business.Utilities
{
    public class UtilityService
    {
        private readonly HttpClient _httpClient;
        public UtilityService(HttpClient httpClient)
        {
            _httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));
        }
        public async Task<ResidentUtilityDto?> GetResidentUtilityBalanceByIdAsync(int customerId)
        {
            var response = await _httpClient.GetAsync($"/balances/v2/{customerId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    return JsonSerializer.Deserialize<ResidentUtilityDto>(await response.Content.ReadAsStringAsync());
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
