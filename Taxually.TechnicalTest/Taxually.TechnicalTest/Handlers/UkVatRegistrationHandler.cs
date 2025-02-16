using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Handlers.Interfaces;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers
{
    public class UkVatRegistrationHandler : IVatRegistrationHandler
    {
        private readonly ITaxuallyHttpClient httpClient;
        public string countryCode { get; } = "GB";

        public UkVatRegistrationHandler(ITaxuallyHttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task RegisterAsync(VatRegistrationRequest request)
        {
            await httpClient.PostAsync("https://api.uktax.gov.uk", request);
        }
    }
}
