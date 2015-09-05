using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Status = CouponStatus.Redeemed;
            this.DateRedeemed = DateTime.Now;
        }
    }
}
