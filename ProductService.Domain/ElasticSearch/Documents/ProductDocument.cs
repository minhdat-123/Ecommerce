using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.ElasticSearch.Documents
{
    public class CompletionField
    {
        public string[] input { get; set; }
    }
    public class ProductDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<int> CategoryPath { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public DateTime CreatedDate { get; set; }
        public CompletionField NameSuggest { get; set; }
    }
}

