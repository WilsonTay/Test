﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public sealed class DealCategoryModel : IAggregateRoot, IEntity
    {
        public string Key { get; internal set; }

        public string Name { get; private set; }

        public string Icon { get; private set; }

        public IList<DealModel> Deals { get; }

        private DealCategoryModel()
        {
            Deals = new List<DealModel>();
        }

        public static DealCategoryModel Create(string categoryName)
        {
            return new DealCategoryModel
            {
                Key = Guid.NewGuid().ToString(),
                Name = categoryName
            };
        }

        public void SetIcon(string icon)
        {
            Icon = icon;
        }

        public void AddDeal(DealModel deal)
        {
            if (Deals.Contains(deal))
            {
                throw new InvalidOperationException("Deal already exist.");
            }

            Deals.Add(deal);
        }

        public void RemoveDeal(DealModel deal)
        {
            Deals.Remove(deal);
        }

        public void SetName(string name)
        {
            Name = name;
        }
    }
}
