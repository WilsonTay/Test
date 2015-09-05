using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public sealed class DealAttributeModel
    {
        public string Name { get; internal set; }

        public string Value { get; internal set; }

        public string Key { get; internal set; }

        public int Order { get; internal set; }
        private DealAttributeModel()
        {

        }

        public static DealAttributeModel Create(string name, string value, int order)
        {
            return new DealAttributeModel
            {
                Name = name,
                Value = value,
                Key = Guid.NewGuid().ToString(),
                Order = order
            };
        }
    }
}
