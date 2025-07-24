namespace AlloyTicketRequestApi.Models
{
    public class AlloyNewTicket
    {
        public int? ActionId { get; set; }
        public object Fields { get; set; } // Now dynamic/object
    }
}
