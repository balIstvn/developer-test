using System.Text;
using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Handlers.Interfaces;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers
{
    public class FranceVatRegistrationHandler : IVatRegistrationHandler
    {
        private readonly ITaxuallyQueueClient excelQueueClient;
        public string countryCode { get; } = "FR";

        public FranceVatRegistrationHandler(ITaxuallyQueueClient queueClient)
        {
            excelQueueClient = queueClient;
        }

        public async Task RegisterAsync(VatRegistrationRequest request)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CompanyName,CompanyId");
            csvBuilder.AppendLine($"{request.CompanyName}{request.CompanyId}");
            var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            // Queue file to be processed
            await excelQueueClient.EnqueueAsync("vat-registration-csv", csv);
        }
    }
}
