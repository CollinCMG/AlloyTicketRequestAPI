using System.Collections.Generic;

namespace AlloyTicketRequestApi.Models
{
    public class PageDto
    {
        public string PageName { get; set; } = string.Empty;
        public int PageRank { get; set; }
        public List<FieldInputDto> Items { get; set; } = new();
    }
}
