using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class MerchantModel : IAggregateRoot, IEntity
    {
        public string Key { get; set; }

        public string EmailAddress { get;  set; }
        public string PhoneNumber { get;  set; }
        public string Website { get;  set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string BusinessRegNumber { get; set; }
        public string About { get; set; }

        public IList<DealModel> Deals { get; private set; }

        public MerchantModel()
        {
            Deals = new List<DealModel>();
        }
    }
}
