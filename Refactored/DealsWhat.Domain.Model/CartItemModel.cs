using System;
using System.Collections.Generic;

namespace DealsWhat.Domain.Model
{
    public class CartItemModel : IEntity
    {
        public DealModel Deal { get; private set; }

        public DealOptionModel DealOption { get; private set; }

        public ICollection<DealAttributeModel> AttributeValues
        {
            get
            {
                return attributeValues;
            }
        }

        private readonly List<DealAttributeModel> attributeValues;

        private CartItemModel()
        {
            attributeValues = new List<DealAttributeModel>();
        }


        public static CartItemModel Create(
            DealModel deal, 
            DealOptionModel dealOption, 
            List<DealAttributeModel> selectedAttributeValues)
        {
            var cartItemModel = new CartItemModel
            {
                Deal = deal,
                DealOption = dealOption
            };

            foreach (var attr in selectedAttributeValues)
            {
                cartItemModel.attributeValues.Add(attr);
            }

            cartItemModel.Key = Guid.NewGuid().ToString();

            return cartItemModel;
        }

        public string Key { get; internal set; }
    }
}