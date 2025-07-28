﻿using System.Collections.Generic;

namespace AlloyTicketRequestApi.Models
{
    public class PageDto
    {
        public string PageName { get; set; } = string.Empty;
        public int PageRank { get; set; }
        public List<FieldInputDto> Items { get; set; } = new();

        // Returns true if all FieldInputDto items are hidden (IsHidden == true), false otherwise
        public bool IsHidden => Items.OfType<FieldInputDto>().Any() && Items.OfType<FieldInputDto>().All(f => f.IsHidden);
    }
}
