using AlloyTicketRequestApi.Models;
using AlloyTicketRequestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlloyTicketRequestApi.Controllers
{
    [ApiController]
    [Route("request")]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequestAsync([FromBody] RequestActionPayload request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request payload.");
            }

            if (string.IsNullOrWhiteSpace(request.Requester_ID))
            {
                return BadRequest("Requester_Id must be set");
            }

            return await _requestService.ProcessRequestAsync(request);
        }
    }
}
