using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model.Exceptions;

namespace DealsWhat.Domain.Model
{
    public class CouponModel : IEntity, IAggregateRoot
    {
        public string Key
        {
            get; private set;
        }

        public string Value { get; private set; }

        public DateTime? DateRedeemed { get; private set; }

        public CouponStatus Status { get; private set; }

        private CouponModel() { }

        public static CouponModel Create(string value)
        {
            var model = new CouponModel();
            model.Key = Guid.NewGuid().ToString();
            model.Value = value;
            model.Status = CouponStatus.Unredeemed;

            return model;
        }

        public void SetRedeemed()
        {
            if (this.Status == CouponStatus.Redeemed)
            {
                throw new CouponAlreadyRedeemedException();
            }

            this.Status = CouponStatus.Redeemed;
            this.DateRedeemed = DateTime.Now;
        }
    }
}
