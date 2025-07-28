using AlloyTicketRequestApi.Models;

namespace AlloyTicketRequestApi.Services
{
    public interface IFormFieldService
    {
        Task<Guid> GetFormIdByObjectId(string objectId);
        Task<Guid> GetFormIdByActionId(int? actionId);
    }
}
