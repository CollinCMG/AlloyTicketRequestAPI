using System.Text.Json;

namespace AlloyTicketRequestApi.Models
{
    public class RequestActionPayload
    {
        public string Requester_ID { get; set; }
        public JsonElement Data { get; set; }
        public string ObjectId { get; set; }
        public string Route { get; set; }
    }
}
