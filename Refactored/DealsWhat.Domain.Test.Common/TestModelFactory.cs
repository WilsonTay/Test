using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DealsWhat.Domain.Test.Common
{
    public static class TestModelFactory
    {
        private static readonly IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

        public static DealModel CreateCompleteDeal(
            string shortTitle = "",
            string shortDescription = "",
            string longTitle = "",
            string longDescription = "",
            string finePrint = "",
            string highlight = "",
            string canonicalUrl = "",
            string id = "")
        {
            var deal = CreateDeal(shortTitle, shortDescription, longTitle, longDescription, finePrint, highlight, canonicalUrl, id);

            for (int i = 1; i < 5; i++)
            {
                deal.AddImage(CreateDealImage());
            }

            for (int i = 1; i < 5; i++)
            {
                deal.AddOption(CreateDealOptionWithAttributes());
            }

            return deal;
        }

        public static DealModel CreateDeal(
            string shortTitle = "",
            string shortDescription = "",
            string longTitle = "",
            string longDescription = "",
            string finePrint = "",
            string highlight = "",
            string canonicalUrl = "",
            string id = "")
        {
            var deal = DealModel.Create(
                shortTitle.Equals("") ? fixture.Create<string>() : shortTitle,
                shortDescription.Equals("") ? fixture.Create<string>() : shortDescription,
                longTitle.Equals("") ? fixture.Create<string>() : longTitle,
                longDescription.Equals("") ? fixture.Create<string>() : longDescription,
                finePrint.Equals("") ? fixture.Create<string>() : finePrint,
                highlight.Equals("") ? fixture.Create<string>() : highlight);

            deal.Key = string.IsNullOrEmpty(id) ? fixture.Create<string>() : id;

            if (!string.IsNullOrEmpty(canonicalUrl))
            {
                deal.CanonicalUrl = canonicalUrl;
            }

            var regularPrice = fixture.Create<double>();
            var specialPrice = fixture.Create<double>(regularPrice);

            deal.SetPrice(regularPrice, specialPrice);

            return deal;
        }

        public static UserModel CreateUser(string key = "", string emailAddress = "", AddressModel contactAddress = null, AddressModel billingAddress = null, string username = "")
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                emailAddress = string.Format("{0}@{1}.com", fixture.Create<string>(), fixture.Create<string>());
            }

            if (string.IsNullOrEmpty(username))
            {
                username = emailAddress;
            }

            var user = UserModel.Create(emailAddress, username);

            user.SetContactAddress(contactAddress ?? CreateAddress());
            user.SetBillingAddress(billingAddress ?? CreateAddress());

            user.Key = string.IsNullOrEmpty(key) ? fixture.Create<string>() : key;

            return user;
        }

        public static AddressModel CreateAddress(
            string key = null,
            string line1 = null,
            string line2 = null,
            string postCode = null,
            string city = null,
            string state = null,
            string country = null)
        {
            return new AddressModel
            {
                Key = key ?? fixture.Create<string>(),
                Line1 = line1 ?? fixture.Create<string>(),
                Line2 = line2 ?? fixture.Create<string>(),
                PostCode = postCode ?? fixture.Create<string>(),
                City = city ?? fixture.Create<string>(),
                State = state ?? fixture.Create<string>(),
                Country = country ?? fixture.Create<string>(),
            };
        }

        public static NewCartItemModel CreateNewCartItem(
            string dealId = null,
            string dealOptionId = null,
            IList<string> selectedAttributesId = null)
        {
            if (string.IsNullOrWhiteSpace(dealId))
            {
                dealId = fixture.Create<string>();
            }

            if (string.IsNullOrWhiteSpace(dealOptionId))
            {
                dealOptionId = fixture.Create<string>();
            }

            if (selectedAttributesId == null)
            {
                selectedAttributesId = new List<string>();
                var random = new Random().Next(0, 3);

                for (int i = 0; i < random; i++)
                {
                    selectedAttributesId.Add(fixture.Create<string>());
                }
            }

            return NewCartItemModel.Create(dealId, dealOptionId, selectedAttributesId);
        }

        public static CartItemModel CreateCartItem(
            DealModel deal = null,
            DealOptionModel dealOption = null,
            List<DealAttributeModel> dealAttributes = null)
        {
            if (deal == null && dealOption == null)
            {
                deal = CreateCompleteDeal();
                dealOption = deal.Options.First();
            }

            if (dealAttributes == null)
            {
                dealAttributes = new List<DealAttributeModel>();
                foreach (var attr in dealOption.Attributes)
                {
                    dealAttributes.Add(attr);
                }
            }

            return CartItemModel.Create(deal, dealOption, dealAttributes);
        }

        public static DealCategoryModel CreateDealCategory(string key = "", string name = "")
        {
            var category = DealCategoryModel.Create(name.Equals("") ? fixture.Create<string>() : name);

            category.Key = string.IsNullOrEmpty(key) ? fixture.Create<string>() : key;

            return category;
        }

        public static DealImageModel CreateDealImage(string relativeUrl = "", int order = -1)
        {
            if (string.IsNullOrEmpty(relativeUrl))
            {
                relativeUrl = fixture.Create<string>() + ".jpg";
            }

            if (order == -1)
            {
                order = fixture.Create<int>();
            }

            return DealImageModel.Create(relativeUrl, order);
        }

        public static DealOptionModel CreateDealOption(
            string shortTitle = "",
            double regularPrice = 0,
            double specialPrice = 0)
        {
            if (regularPrice == 0 && specialPrice == 0)
            {
                regularPrice = fixture.Create<double>();
                specialPrice = fixture.Create<double>(regularPrice);
            }

            var dealOption = DealOptionModel.Create(
                shortTitle.Equals("") ? fixture.Create<string>() : shortTitle,
                regularPrice,
                specialPrice);

            return dealOption;
        }

        public static DealOptionModel CreateDealOptionWithAttributes(
            string shortTitle = "",
            double regularPrice = 0,
            double specialPrice = 0,
            int optionsToCreate = 3)
        {
            var dealOption = CreateDealOption(shortTitle, regularPrice, specialPrice);

            for (int i = 0; i < optionsToCreate; i++)
            {
                var attribute = CreateDealAttribute();

                dealOption.AddAttribute(attribute);
            }

            return dealOption;
        }

        public static DealAttributeModel CreateDealAttribute(string name = "", string value = "", int order = -1)
        {
            return DealAttributeModel.Create(
                name.Equals("") ? fixture.Create<string>() : name,
                value.Equals("") ? fixture.Create<string>() : value,
                order == -1 ? fixture.Create<int>() : order);
        }
    }
}
