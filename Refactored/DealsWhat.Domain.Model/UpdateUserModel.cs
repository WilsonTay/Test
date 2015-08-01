using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class UpdateUserModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public AddressModel BillingAddress { get; set; }

        public AddressModel ContactAddress { get; set; }
    }
}
