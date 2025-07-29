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
            var (flowControl, value, token) = await AuthenticateWithAlloy();
            if (!flowControl)
            {
                return value;
            }

            try
            {
                if (request.Type == null)
                {
                    return new BadRequestObjectResult("Type must be set");
                }

                if (request.Type == RequestType.Service)
                {
                    await _alloyService.CreateAlloyServiceRequestAsync(token.AccessToken, request);

                }
                else if (request.Type == RequestType.Support)
                {
                    if (request.ActionId == null)
                    {
                        return new BadRequestObjectResult("Invalid action id");
                    }

                    await _alloyService.CreateAlloySupportRequestAsync(token.AccessToken, request);
                }
                else
                {
                    return new BadRequestObjectResult("Invalid request type");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Error creating Alloy service request. " + ex.Message);
            }

            return new OkObjectResult("");
        }

        private async Task<(bool flowControl, IActionResult? value, AlloyToken? token)> AuthenticateWithAlloy()
        {
            try
            {
                var token = await _alloyService.AuthenticateWithAlloyAsync();
                if (token == null || token.AccessToken == null)
                {
                    return (flowControl: false, value: new StatusCodeResult(500), token: null);
                }
                return (flowControl: true, value: null, token: token);
            }
            catch (Exception ex)
            {
                return (flowControl: false, value: new BadRequestObjectResult("Unable to retrieve access token from Alloy. " + ex.Message), token: null);
            }
        }
    }
}
