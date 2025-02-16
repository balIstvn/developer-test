using System.Xml.Serialization;
using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Handlers.Interfaces;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers
{
    public class GermanyVatRegistrationHandler : IVatRegistrationHandler
    {
        private readonly ITaxuallyQueueClient xmlQueueClient;
        public string countryCode { get; } = "DE";

        public GermanyVatRegistrationHandler(ITaxuallyQueueClient queueClient)
        {
            xmlQueueClient = queueClient;
        }

        public async Task RegisterAsync(VatRegistrationRequest request)
        {
            using (var stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
                serializer.Serialize(stringwriter, request);
                var xml = stringwriter.ToString();
                // Queue xml doc to be processed
                await xmlQueueClient.EnqueueAsync("vat-registration-xml", xml);
            }
        }
    }
}
