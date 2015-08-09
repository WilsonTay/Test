using System.Collections.Generic;

namespace DealsWhat.Application.WebApi.Models
{
    public class CartItemViewModel
    {
        public string ShortName { get; set; }
        public double RegularPrice { get; set; }
        public double SpecialPrice { get; set; }
        public string Id { get; set; }

        public IList<CartItemAttribute> Attributes { get; }
        public string DealId { get; set; }
        public string DealThumbnailUrl { get; set; }

        public string DealUrl { get; set; }

        public int Quantity { get; set; }

        public CartItemViewModel()
        {
            Attributes = new List<CartItemAttribute>();
        }
    }
}