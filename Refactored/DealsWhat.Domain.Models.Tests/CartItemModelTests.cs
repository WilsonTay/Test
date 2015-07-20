using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DealsWhat.Domain.Models.Tests
{
    [TestClass]
    public class CartItemModelTests
    {
        [TestMethod]
        public void Create_IdGenerated()
        {
            var deal = TestModelFactory.CreateCompleteDeal();
            var dealOption = deal.Options.First();
            var attrValues = new List<DealAttributeModel>();
            attrValues.Add(dealOption.Attributes.First());

            var cartItem = CartItemModel.Create(deal, dealOption, attrValues);

            cartItem.Key.Should().NotBeNull();
        }

        [TestMethod]
        public void Create_AllFieldMatches()
        {
            var deal = TestModelFactory.CreateCompleteDeal();
            var dealOption = deal.Options.First();
            var attrValues = new List<DealAttributeModel>();
            attrValues.Add(dealOption.Attributes.First());

            var cartItem = CartItemModel.Create(deal, dealOption, attrValues);

            cartItem.DealOption.ShouldBeEquivalentTo(dealOption);
            cartItem.AttributeValues.Should().Contain(attrValues[0]);
        }

    }
}
