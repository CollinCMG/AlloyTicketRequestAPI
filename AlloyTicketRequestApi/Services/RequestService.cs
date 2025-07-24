using AlloyTicketRequestApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlloyTicketRequestApi.Services
{
    public interface IRequestService
    {
        Task<IActionResult> ProcessRequestAsync(RequestActionPayload request);
    }

    public class RequestService : IRequestService
    {
        private readonly AlloyService _alloyService;
        private readonly ILogger<RequestService> _logger;

        public RequestService(AlloyService alloyService, ILogger<RequestService> logger)
        {
            _alloyService = alloyService;
            _logger = logger;
        }
        public async Task<IActionResult> ProcessRequestAsync(RequestActionPayload request)
        {
            AlloyToken? token;
            try
            {
                token = await _alloyService.AuthenticateWithAlloyAsync();
                if (token == null || token.access_token == null)
                {
                    return new StatusCodeResult(500);
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Unable to retrieve access token from Alloy. " + ex.Message);
            }

            try
            {
                await _alloyService.CreateAlloyRequestAsync(token.access_token, request);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Error creating Alloy service request. " + ex.Message);
            }

            return new OkObjectResult("");
        }
    }
}
