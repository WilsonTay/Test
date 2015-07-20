using System.Collections.Generic;

namespace DealsWhat.Application.WebApi.Models
{
    public class NewCartItemViewModel
    {
        public string DealId { get; set; }
        public string DealOptionId { get; set; }
        public IList<string> SelectedAttributes { get; set; }
    }
}