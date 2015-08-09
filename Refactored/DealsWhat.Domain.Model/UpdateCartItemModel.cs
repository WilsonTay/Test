using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class UpdateCartItemModel
    {
        public string Key { get; internal set; }
        public int Quantity { get; internal set; }

        private UpdateCartItemModel()
        {
            
        }
        public static UpdateCartItemModel Create(string key, int quantity)
        {
            return new UpdateCartItemModel
            {
                Key = key,
                Quantity = quantity
            };
        }
    }
}
