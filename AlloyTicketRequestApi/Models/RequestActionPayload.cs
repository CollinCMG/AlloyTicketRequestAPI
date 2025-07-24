using System.Text.Json;

namespace AlloyTicketRequestApi.Models
{
    public enum RequestType
    {
        Service,
        Support
    }

    public class RequestActionPayload
    {
        public string Requester_ID { get; set; }
        public JsonElement Data { get; set; }
        public string ObjectId { get; set; }
        public int? ActionId  { get; set; }
        public RequestType? Type  { get; set; }
    }
}
