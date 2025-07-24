using AlloyTicketRequestApi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AlloyTicketRequestApi.Services
{
    public class AlloyService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly ILogger<AlloyService> _logger;

        public AlloyService(HttpClient client, IConfiguration config, ILogger<AlloyService> logger)
        {
            _client = client;
            _config = config;
            _logger = logger;
        }

        public async Task<AlloyToken?> AuthenticateWithAlloyAsync()
        {
            AlloyToken? token = new AlloyToken();
            try
            {
                var postObj = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", _config.GetSection("Alloy")["Username"] },
                    { "password", _config.GetSection("Alloy")["Password"] }
                };
                var authContent = new FormUrlEncodedContent(postObj);

                using (var response = await _client.PostAsync(_config.GetSection("Alloy")["BaseUrl"] + "/API/token", authContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<AlloyToken>(apiResponse);
                }

                if (token == null)
                {
                    throw new Exception("HttpClient returned without error but token is null.");
                }

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError("HttpClient returned without error but token is null.");
                throw new Exception("HttpClient returned without error but token is null.");
            }
        }

        public async Task<bool> CreateAlloyRequestAsync(string accessToken, RequestActionPayload request)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Use JObject for concise property override
            object combinedObj;
            if (request.Data.ValueKind == System.Text.Json.JsonValueKind.Object)
            {
                // Convert JsonElement to JObject
                var jObj = Newtonsoft.Json.Linq.JObject.Parse(request.Data.GetRawText());
                jObj["Requester_ID"] = request.Requester_ID == null ? null : new Newtonsoft.Json.Linq.JValue(request.Requester_ID);
                combinedObj = jObj;
            }
            else
            {
                // If Data is not an object, just add Requester_ID and Data as properties
                combinedObj = new { Requester_ID = request.Requester_ID, Data = request.Data };
            }

            string jsonToSend = JsonConvert.SerializeObject(combinedObj);

            StringContent newSRContent = new StringContent(jsonToSend, Encoding.UTF8, "application/json");
            using var response = await _client.PostAsync(
                _config.GetSection("Alloy")["BaseUrl"] + "/api/v2/object/" + request.ObjectId + "/action/131",
                newSRContent);

            string apiResponse = await response.Content.ReadAsStringAsync();
            dynamic? respObj = JsonConvert.DeserializeObject(apiResponse);

            if (respObj == null)
            {
                throw new Exception("Error communicating with Alloy. Response object is NULL.");
            }
            if (respObj.success != "true")
            {
                string errorCode = Convert.ToString(respObj.errorCode) ?? string.Empty;
                string errorText = Convert.ToString(respObj.errorText) ?? string.Empty;
                _logger.LogError("Request unsuccessful. ({ErrorCode}) {ErrorText}", errorCode, errorText);
                throw new Exception("Request unsuccessful. (" + errorCode + ") " + errorText);
            }

            return true;
        }
    }
}
