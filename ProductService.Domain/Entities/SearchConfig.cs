using System;
using System.Collections.Generic;

namespace ProductService.Domain.Entities
{
    public class SearchConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SearchConfigType Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public enum SearchConfigType
    {
        Synonym = 1,
        Stemming = 2
    }
}
