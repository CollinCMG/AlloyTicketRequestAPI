namespace AlloyTicketRequestApi.Models
{
    public class AlloyToken
    {
        public string? AccessToken { get; set; }
        public string? TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
}
