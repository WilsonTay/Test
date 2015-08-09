using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsWhat.Application.WebApi.Models
{
    public class UpdateCartItemViewModel
    {
        public IList<SingleUpdateCartItemViewModel> UpdateCartItem { get; set; }

        public UpdateCartItemViewModel()
        {
            UpdateCartItem = new List<SingleUpdateCartItemViewModel>();
        }
    }
}