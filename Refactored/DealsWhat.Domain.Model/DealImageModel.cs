﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public sealed class DealImageModel
    {
        public string Key { get; internal set; }
        public string RelativeUrl { get; private set; }
        public int Order { get; private set; }

        private DealImageModel()
        {

        }

        public static DealImageModel Create(string relativeUrl, int order)
        {
            return new DealImageModel
            {
                RelativeUrl = relativeUrl,
                Order = order,
                Key = Guid.NewGuid().ToString()
            };
        }

        public void SetOrder(int order)
        {
            Order = order;
        }
    }
}
