using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model.Exceptions
{
    public class CouponNotFoundException : Exception
    {
        public CouponNotFoundException()
        {
        }

        public CouponNotFoundException(string message) : base(message)
        {
        }

        public CouponNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
