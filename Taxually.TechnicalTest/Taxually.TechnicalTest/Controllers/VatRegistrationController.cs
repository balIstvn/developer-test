using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Handlers.Interfaces;
using Taxually.TechnicalTest.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        private readonly Dictionary<string, IVatRegistrationHandler> registrationHandlers;

        public VatRegistrationController(IEnumerable<IVatRegistrationHandler> handlers)
        {
            // Az országkódok alapján készítünk egy szótárat
            registrationHandlers = handlers.ToDictionary(handler => handler.countryCode, handler => handler);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
        {
            if (registrationHandlers.TryGetValue(request.Country, out var requestHandler))
            {
                await requestHandler.RegisterAsync(request);
                return Ok(new { Message = "VAT registration request processed successfully", Country = request.Country });
            }

            return BadRequest("Given country code currently not supported for registration");
        }
    }
}
