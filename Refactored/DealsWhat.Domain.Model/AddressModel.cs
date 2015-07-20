using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class AddressModel : IEntity
    {
        public string Key { get; internal set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public AddressModel()
        {
            Key = Guid.NewGuid().ToString();
        }
    }
}
