using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DealsWhat.Domain.Model;
using DealsWhat.Infrastructure.DataAccess;

namespace DealsWhat.Application.WebApi.Models
{
    public class SampleData
    {
        public static void Seed(DealsWhatDbContext context)
        {

            var categories = CreateDealCategories(context);
            var merchants = CreateMerchants(context);
            var deals = CreateDeals(context, merchants, categories);
            var users = CreateUsers(context);
            CreateDealOptions(context, deals);
            // CreateDealComments(context, deals, users);
            CreateDealImages(context, deals);

            context.SaveChanges();
        }

        private static void CreateDealOptions(DealsWhatDbContext context, List<DealModel> deals)
        {
            var random = new Random();
            var randomNumber = random.Next(0, 5);

            foreach (var deal in deals)
            {
                for (int i = 0; i < randomNumber; i++)
                {

                    var shortTitle = string.Format("Option {0}", i);
                    var regularPrice = 10;
                    var specialPrice = 5;

                    var dealOption = DealOptionModel.Create(shortTitle, regularPrice, specialPrice);

                    // dealOption.Attributes = new List<DealAttribute>();
                    var dealAttribute1 = DealAttributeModel.Create("Color", "Black");
                    var dealAttribute2 = DealAttributeModel.Create("Color", "Blue");
                    var dealAttribute3 = DealAttributeModel.Create("Color", "Red");
                    var dealAttribute4 = DealAttributeModel.Create("Size", "S");
                    var dealAttribute5 = DealAttributeModel.Create("Size", "M");
                    var dealAttribute6 = DealAttributeModel.Create("Size", "L");

                    dealOption.AddAttribute(dealAttribute1);
                    dealOption.AddAttribute(dealAttribute2);
                    dealOption.AddAttribute(dealAttribute3);
                    dealOption.AddAttribute(dealAttribute4);
                    dealOption.AddAttribute(dealAttribute5);
                    dealOption.AddAttribute(dealAttribute6);

                    deal.AddOption(dealOption);
                }
            }
        }

        private static IList<ApplicationUser> CreateUsers(DealsWhatDbContext context)
        {
            var users = new List<ApplicationUser>();

            for (int i = 0; i < 10; i++)
            {
                //var user = new ApplicationUser();
                //user.UserName = string.Format("User{0}", i);
                //user.Email = string.Format("user{0}@email.com", i);
                //user.Id = i.ToString();
                //user.EmailConfirmed = true;

                //var userName = string.Format("User{0}", i);
                //var user = UserModel.Create(userName, userName);
                //user.SetFirstName("first");
                //user.SetLastName("last");

                var user = new ApplicationUser
                {
                    Email = string.Format("User{0}", i),
                    UserName = string.Format("User{0}", i)
                };
                // var applicationUser = new ApplicationUser(user);

                //var user = new ApplicationUser
                //{
                //    UserId = i,
                //    Username = string.Format("User{0}", i),
                //    EmailAddress = string.Format("user{0}@email.com", i),
                //    AddressLine1 = "addressline1",
                //    State = "state",
                //    City = "city",
                //    Street = "street",
                //    FirstName = "firstname",
                //    LastName = "lastname",
                //    ZipCode = "zip"
                //};

                users.Add(user);
                context.Users.Add(user);
            }

            return users;
        }

        //private static void CreateDealComments(DealsWhatUnitOfWork context, IList<DealModel> deals, IList<User> users)
        //{
        //    var random = new Random();

        //    foreach (var deal in deals)
        //    {
        //        for (int i = 0; i < 3; i++)
        //        {
        //            var randomUser = users[random.Next(0, users.Count)];

        //            var comment = new DealComment
        //            {
        //                Id = Guid.NewGuid(),
        //                DatePosted = DateTime.UtcNow,
        //                Message = string.Format("Message {0}", i),
        //                Poster = randomUser,
        //                Deal = deal
        //            };

        //            context.DealComments.Add(comment);
        //        }
        //    }
        //}

        private static void CreateDealImages(DealsWhatDbContext context, IList<DealModel> deals)
        {
            var random = new Random();

            foreach (var deal in deals)
            {
                for (int i = 0; i < 3; i++)
                {
                    var randNumber = random.Next(0, 18);
                    //var image = new DealImageModel
                    //{
                    //    Id = Guid.NewGuid(),
                    //    Order = i,
                    //    RelativeUrl = string.Format("{0}.jpg", randNumber),
                    //    Deal = deal
                    //};

                    var url = string.Format("{0}.jpg", randNumber);
                    var order = i;
                    var image = DealImageModel.Create(url, i);
                    deal.AddImage(image);

                    context.DealImages.Add(image);
                }
            }
        }

        private static IList<MerchantModel> CreateMerchants(DealsWhatDbContext context)
        {
            var merchants = new List<MerchantModel>();

            for (int i = 0; i < 10; i++)
            {
                var merchant = new MerchantModel
                {
                    BusinessRegNumber = "RegNumber",
                    EmailAddress = "email@email.com",
                    Key = Guid.NewGuid().ToString(),
                    Website = "http://www.website.com",
                    PhoneNumber = "010-20318122",
                    Name = string.Format("Merchant {0}", i),
                    About = "About merchant",
                    Address = "address"
                };

                merchants.Add(merchant);
                context.Merchants.Add(merchant);
            }

            return merchants;
        }

        private static IList<DealCategoryModel> CreateDealCategories(DealsWhatDbContext context)
        {
            var categories = new string[] { "Food & Drink", "Beauty & Wellness", "Travel", "Goods", "Shopping" };
            var dealCategories = new List<DealCategoryModel>();

            foreach (var category in categories)
            {
                var dealCategory = DealCategoryModel.Create(category);

                dealCategories.Add(dealCategory);
                context.DealCategories.Add(dealCategory);
            }

            return dealCategories;
        }

        private static string GetDescription()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/SampleDescription.txt");
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                return reader.ReadToEnd();
            }

        }
        private static string GetFinePrint()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/SampleFinePrint.txt");
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                return reader.ReadToEnd();
            }

        }
        private static string GetHighlight()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/SampleHighlight.txt");
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                return reader.ReadToEnd();
            }

        }

        private static List<DealModel> CreateDeals(DealsWhatDbContext context, IList<MerchantModel> merchants, IList<DealCategoryModel> categories)
        {
            var description = GetDescription();
            var fineprint = GetFinePrint();
            var highlight = GetHighlight();

            var random = new Random();

            var deals = new List<DealModel>();

            for (int i = 0; i < 200; i++)
            {
                var isFeatured = false;

                if (i < 3)
                {
                    isFeatured = true;
                }

                var randomCategory = categories[random.Next(0, categories.Count)];
                 var randomMerchant = merchants[random.Next(0, merchants.Count)];

                var shortTitle = string.Format("Sukiyaki Buffet Lunch at IOI Mall Puchong {0}{1}", randomCategory.Name,
                    i.ToString());
                var shortDescription = "Authentic Japanese sukiyaki in a casual dining atmosphere.";
                var canonicalUrl = string.Format("url-for-product-{0}", i);
                var longTitle = "[41% Off] Sukishi: Sukiyaki Buffet Lunch at IOI Mall Puchong for RM24.90";

                var deal = DealModel.Create(shortTitle, shortDescription, longTitle, description, fineprint, highlight);
                deal.SetPrice(10, 5.5);
                deal.SetStatus(DealStatus.Published);
                //var deal = new DealModel
                //{
                //    Id = Guid.NewGuid(),
                //    SKU = string.Format("SKU{0}{1}", randomCategory.Name, i.ToString()),
                //    RegularPrice = 10,
                //    SpecialPrice = 5.50,
                //    ShortTitle = string.Format("Sukiyaki Buffet Lunch at IOI Mall Puchong {0}{1}", randomCategory.Name, i.ToString()),
                //    FinePrint = fineprint,
                //    Highlight = highlight,
                //    StartTime = DateTime.UtcNow,
                //    EndTime = DateTime.UtcNow.AddDays(30),
                //    IsFeatured = isFeatured,
                //    DateAdded = DateTime.UtcNow,
                //    LongTitle = "[41% Off] Sukishi: Sukiyaki Buffet Lunch at IOI Mall Puchong for RM24.90",
                //    LongDescription = description,
                //    ShortDescription = "Authentic Japanese sukiyaki in a casual dining atmosphere.",
                //    CanonicalUrl = string.Format("url-for-product-{0}", i),
                //    Category = randomCategory,
                //    Status = DealStatus.Published
                //};

                randomCategory.AddDeal(deal);
                randomMerchant.Deals.Add(deal);

                deals.Add(deal);
                context.Deals.Add(deal);
            }

            return deals;
        }
    }
}