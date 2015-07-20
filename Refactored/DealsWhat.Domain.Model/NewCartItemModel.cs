using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    /// <summary>
    /// New class instead of the actual CartItemModel because when caller create
    /// a new cart item he might not have DealOption and DealAttributes model.
    /// </summary>
    public class NewCartItemModel
    {
        /// <summary>
        /// Having this because Deal Option and Attributes are not aggregate root.
        /// We can't query them directly so we need the deal id first.
        /// </summary>
        public string DealId { get; set; }

        public string DealOptionId { get; set; }

        public IEnumerable<string> SelectedAttributes
        {
            get
            {
                return selectedAttributes;
            }
        }

        private IList<string> selectedAttributes;

        private NewCartItemModel()
        {
            selectedAttributes = new List<string>();
        }

        public void AddSelectedAttributeId(string attributeId)
        {
            selectedAttributes.Add(attributeId);
        }

        public static NewCartItemModel Create(
            string dealId,
            string dealOptionId,
            IEnumerable<string> selectedAttributes)
        {
            var model = new NewCartItemModel()
            {
                DealId = dealId,
                DealOptionId = dealOptionId,
                selectedAttributes = selectedAttributes.ToList()
            };

            return model;
        }
    }
}
