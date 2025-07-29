using AlloyTicketRequestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlloyTicketRequestApi.Controllers
{
    [ApiController]
    [Route("formfields")]
    [Authorize]
    public class FormFieldController : ControllerBase
    {
        private readonly IFormFieldService _formFieldService;

        public FormFieldController(IFormFieldService formFieldService)
        {
            _formFieldService = formFieldService;
        }

        [HttpGet("object/{objectId}")]
        public async Task<IActionResult> GetFormIdByObjectId(string objectId)
        {
            if (string.IsNullOrWhiteSpace(objectId))
                return BadRequest("ObjectId must be provided.");

            var formId = await _formFieldService.GetFormIdByObjectId(objectId);
            if (formId == Guid.Empty)
                return NotFound();

            return Ok(formId);
        }

        [HttpGet("action/{actionId}")]
        public async Task<IActionResult> GetFormIdByActionId(int? actionId)
        {
            if (actionId == null)
                return BadRequest("ActionId must be provided.");

            var formId = await _formFieldService.GetFormIdByActionId(actionId);
            if (formId == Guid.Empty)
                return NotFound();

            return Ok(formId);
        }

        [HttpGet("pages/{formId}")]
        public async Task<IActionResult> GetFormPages(Guid formId)
        {
            if (formId == Guid.Empty)
                return BadRequest("FormId must be provided.");

            // Cast to concrete type to access GetFormPagesAsync
            if (_formFieldService is not Services.FormFieldService concreteService)
                return StatusCode(500, "Service implementation error.");

            var pages = await concreteService.GetFormPagesAsync(formId);
            return Ok(pages);
        }
    }
}
