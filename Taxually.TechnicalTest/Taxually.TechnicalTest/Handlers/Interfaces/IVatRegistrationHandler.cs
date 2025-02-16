using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers.Interfaces
{
    public interface IVatRegistrationHandler
    {
        string countryCode { get; }
        Task RegisterAsync(VatRegistrationRequest request);
    }
}
