using System;
using System.Collections.Generic;
using DealsWhat.Domain.Model;

namespace DealsWhat.Application.WebApi.Models
{
    public class OrderViewModel
    {
        public string Id { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public IList<OrderlineViewModel> Orderlines { get; set; }

        public IList<string> AddressLines { get; set; }

        public DateTime DatePlaced { get; set; }

        public double TotalSpecialPrice { get; set; }

        public OrderViewModel()
        {
            Orderlines = new List<OrderlineViewModel>();
            AddressLines = new List<string>();
        }
    }
}