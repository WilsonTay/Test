﻿using System;
using System.Linq;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DealsWhat.Domain.Models.Tests
{
    [TestClass]
    public class DealTests
    {
        private IFixture fixture;
        private string shortTitle = "short title";
        private string longTitle = "long title";
        private string shortDescription = "short description";
        private string longDescription = "long description";
        private string finePrint = "fine print";
        private string highlight = "highlight";
        private double regularPrice = 15.00;
        private double specialPrice = 7.50;
        private DateTime startTime;
        private DateTime endTime;

        [TestInitialize]
        public void TestInitialize()
        {
            startTime = DateTime.Now;
            endTime = DateTime.Now.AddDays(7);

            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void CreateDeal_AllAttributesSet()
        {
            var deal = DealModel.Create(
                shortTitle,
                shortDescription,
                longTitle,
                longDescription,
                finePrint,
                highlight,
                DealType.Product);

            deal.ShortTitle.Should().Be(shortTitle);
            deal.ShortDescription.Should().Be(shortDescription);
            deal.LongTitle.Should().Be(longTitle);
            deal.LongDescription.Should().Be(longDescription);
            deal.FinePrint.Should().Be(finePrint);
            deal.Highlight.Should().Be(highlight);

            deal.RegularPrice.Should().Be(0.0);
            deal.SpecialPrice.Should().Be(0.0);

            deal.DateAdded.Should().BeCloseTo(DateTime.UtcNow, 20);
            deal.StartTime.Should().BeCloseTo(DateTime.UtcNow, 20);
            deal.EndTime.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), 20);

            deal.IsFeatured.Should().BeFalse();
            deal.Status.ShouldBeEquivalentTo(DealStatus.Draft);
            deal.Key.Should().BeOfType<string>();
            deal.SKU.Should().NotBeEmpty();

            deal.Images.Should().BeEmpty();
        }

        [TestMethod]
        public void CreateDeal_SKU_ShouldNotContainSpecialCharacter()
        {
            var deal = TestModelFactory.CreateDeal(shortTitle: "BBQ in Puchong & Sunway");

            deal.SKU.Should().NotContain("&");
        }

        [TestMethod]
        public void CreateDeal_CanonicalUrl_ShouldNotContainSpecialCharacter2()
        {
            var deal = TestModelFactory.CreateDeal(shortTitle: "BBQ in Puchong & Sunway !@#$%^&*()");

            "!@#$%^&* ()".ToList().ForEach(a =>
            {
                deal.CanonicalUrl.Should().NotContain(a.ToString());
            });
        }

        [TestMethod]
        public void CreateDeal_CanonicalUrl_SpaceShouldBeConvertedToDash()
        {
            var shorTitle = "this is a deal";
            var shortTitleWithDash = shorTitle.Replace(" ", "-");

            var deal = TestModelFactory.CreateDeal(shortTitle: shorTitle);

            deal.CanonicalUrl.Should().Contain(shortTitleWithDash);
        }

        [TestMethod]
        public void CreateDeal_CanonicalUrl_ShouldNotContainSpecialCharacter()
        {
            var deal = TestModelFactory.CreateDeal(shortTitle: "BBQ in Puchong & Sunway");

            deal.CanonicalUrl.Should().NotContain("&");
        }

        [TestMethod]
        public void CreateDeal_SKU_ShouldNotContainSpecialCharacter2()
        {
            var deal = TestModelFactory.CreateDeal(shortTitle: "BBQ in Puchong & Sunway !@#$%^&* ()");

            "!@#$%^&* ()".ToList().ForEach(a =>
            {
                deal.SKU.Should().NotContain(a.ToString());
            });
        }

        [TestMethod]
        public void AddImages_ImagesAdded()
        {
            var images = fixture.CreateMany<DealImageModel>(10);
            var deal = TestModelFactory.CreateDeal();

            foreach (var image in images)
            {
                deal.AddImage(image);
            }

            foreach (var image in images)
            {
                deal.Images.Should().Contain(image);
            }
        }

        [TestMethod]
        public void SetPrice_PriceSetCorrectly()
        {
            var deal = TestModelFactory.CreateDeal();

            deal.SetPrice(20, 15);

            deal.RegularPrice.ShouldBeEquivalentTo(20);
            deal.SpecialPrice.ShouldBeEquivalentTo(15);
        }

        [TestMethod]
        public void AddDealOptions_OptionsAreAdded()
        {
            var options = Enumerable.Range(0, 10).Select(a => TestModelFactory.CreateDealOption()).ToList();
            var deal = TestModelFactory.CreateDeal();

            foreach (var option in options)
            {
                deal.AddOption(option);
            }

            foreach (var option in options)
            {
                deal.Options.Should().Contain(option);
            }
        }
    }
}
