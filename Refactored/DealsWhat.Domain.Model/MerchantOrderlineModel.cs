using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class MerchantOrderlineModel
    {
        public OrderlineModel Orderline { get; private set; }
        public AddressModel DeliveryAddress { get; private set; }
        public DateTime DateCreated { get; private set; }
        
        private MerchantOrderlineModel() { }

        public static  MerchantOrderlineModel Create(OrderlineModel orderline,AddressModel deliveryAddress,DateTime dateCreated)
        {
            var model = new MerchantOrderlineModel();
            model.Orderline = orderline;
            model.DeliveryAddress = deliveryAddress;
            model.DateCreated = dateCreated;

            return model;
        }
    }
}
