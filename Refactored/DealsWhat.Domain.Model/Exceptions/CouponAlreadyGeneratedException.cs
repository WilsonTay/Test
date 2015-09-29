using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model.Exceptions
{
    public sealed class CouponAlreadyGeneratedException : Exception
    {
        public CouponAlreadyGeneratedException()
        {
        }

        public CouponAlreadyGeneratedException(string message) : base(message)
        {
        }

        public CouponAlreadyGeneratedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
