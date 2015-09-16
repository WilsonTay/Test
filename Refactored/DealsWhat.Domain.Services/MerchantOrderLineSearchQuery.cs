using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Services
{
    public class MerchantOrderLineSearchQuery
    {
        public string MerchantId { get; set; }

        public string DealId { get; set; }
    }
}
