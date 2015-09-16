using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model.Exceptions
{
    public sealed class CouponAlreadyRedeemedException : Exception
    {
        public CouponAlreadyRedeemedException()
        {
        }

        public CouponAlreadyRedeemedException(string message) : base(message)
        {
        }

        public CouponAlreadyRedeemedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
