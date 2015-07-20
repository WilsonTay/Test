using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class UpdateUserModel
    {
        public AddressModel NewBillingAddress { get; set; }
        public AddressModel NewContactAddress { get; set; }
        
    }
}
