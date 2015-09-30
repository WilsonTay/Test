using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Interfaces.Helpers;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Services;

namespace DealsWhat.Application.WebApi.Controllers
{
    //[Authorize]
    public class FrontEndDealsController : ApiController
    {
        private readonly IDealService dealService;
        private readonly IOrderService orderService;

        public FrontEndDealsController(IDealService dealService, IOrderService orderService)
        {
            this.dealService = dealService;
            this.orderService = orderService;

            AutoMapper.Mapper.CreateMap<DealModel, FrontEndDeal>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()))
                .AfterMap((dest, src) =>
                {
                    src.ThumbnailUrls = dest.Images.Select(i => ImageHelper.GenerateThumbnailPath(i.RelativeUrl)).ToList();
                });

            AutoMapper.Mapper.CreateMap<DealAttributeModel, FrontEndSpecificDealAttribute>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()));

            AutoMapper.Mapper.CreateMap<DealOptionModel, FrontDealSpecificDealOption>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()))
                .AfterMap((dest, src) =>
                {
                    if (src.DealAttributes.Any())
                    {
                        return;
                    }

                    foreach (var attr in dest.Attributes.OrderBy(a => a.Order))
                    {
                        var converted = AutoMapper.Mapper.Map<FrontEndSpecificDealAttribute>(attr);
                        src.DealAttributes.Add(converted);
                    }
                });

            AutoMapper.Mapper.CreateMap<DealCategoryModel, FrontEndCategoryViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()))
                 .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()));

            AutoMapper.Mapper.CreateMap<DealModel, FrontEndSpecificDeal>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.RelativeUrl)))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .AfterMap((dest, src) =>
                {
                    if (src.DealOptions.Any())
                    {
                        return;
                    }

                    foreach (var option in dest.Options)
                    {
                        var converted = AutoMapper.Mapper.Map<FrontDealSpecificDealOption>(option);
                        src.DealOptions.Add(converted);
                    }
                });
        }

        // GET api/values
        [HttpGet]
        [Route("api/deals/")]
        public IEnumerable<FrontEndDeal> Get()
        {
            var query = Request.GetQueryNameValuePairs().ToList();
            var searchQuery = new DealSearchQuery();

            var searchTerm = query.FirstOrDefault(k => k.Key.Equals("search", StringComparison.OrdinalIgnoreCase));
            var categoryId = query.FirstOrDefault(k => k.Key.Equals("categoryid", StringComparison.OrdinalIgnoreCase));
            var merchantId = query.FirstOrDefault(k => k.Key.Equals("merchantId", StringComparison.OrdinalIgnoreCase));
            var excludeExpired = query.FirstOrDefault(k => k.Key.Equals("excludeExpired", StringComparison.OrdinalIgnoreCase));

            // Category id, search term, sorted by, all
            if (KeyHasValue(categoryId))
            {
                searchQuery.CategoryId = categoryId.Value;
            }

            if (KeyHasValue(searchTerm))
            {
                searchQuery.SearchTerm = searchTerm.Value;
            }

            if (KeyHasValue(merchantId))
            {
                searchQuery.MerchantId = merchantId.Value;
            }

            if (KeyHasValue(excludeExpired))
            {
                searchQuery.ExcludeExpired = excludeExpired.Value.Equals("1");
            }

            //TODO: Combine search term and category.

            var convertedSearchResults = dealService.SearchDeals(searchQuery)
                .Select(d => {
                    var mapped = AutoMapper.Mapper.Map<FrontEndDeal>(d);

                    return mapped;
                })
                .ToList();

            foreach (var result in convertedSearchResults)
            {
                for (int i = 0; i < result.ThumbnailUrls.Count; i++)
                {
                    result.ThumbnailUrls[i] =
                        PathHelper.ConvertRelativeToAbsoluteDealImagePath(result.ThumbnailUrls[i]);
                }
            }

            return convertedSearchResults;
        }

        [HttpGet]
        [Route("api/deal/")]
        public FrontEndSpecificDeal GetSingle()
        {
            var query = Request.GetQueryNameValuePairs().ToList();
            var searchQuery = new SingleDealSearchQuery();

            var dealId = query.FirstOrDefault(k => k.Key.Equals("id", StringComparison.OrdinalIgnoreCase));
            var url = query.FirstOrDefault(k => k.Key.Equals("url", StringComparison.OrdinalIgnoreCase));

            if (KeyHasValue(dealId))
            {
                searchQuery.Id = dealId.Value;
            }

            if (KeyHasValue(url))
            {
                searchQuery.CanonicalUrl = url.Value;
            }

            var searchResult = dealService.SearchSingleDeal(searchQuery);

            var convertedSearchResult = AutoMapper.Mapper.Map<DealModel, FrontEndSpecificDeal>(searchResult);

            for (int i = 0; i < convertedSearchResult.ImageUrls.Count; i++)
            {
                convertedSearchResult.ImageUrls[i] =
                    PathHelper.ConvertRelativeToAbsoluteDealImagePath(convertedSearchResult.ImageUrls[i]);
            }

            var orderCount = this.orderService.GetOrderCount(convertedSearchResult.Id);
            convertedSearchResult.OrderCount = orderCount;

            return convertedSearchResult;
        }


        [HttpGet]
        [Route("api/categories")]
        public IEnumerable<FrontEndCategoryViewModel> GetCategories()
        {
            var categories = this.dealService.GetAllCategories();

            var frontEndCategories = categories.Select(a => Mapper.Map<FrontEndCategoryViewModel>(a)).ToList();

            return frontEndCategories;
        }

        private static bool KeyHasValue(KeyValuePair<string, string> kvp)
        {
            if (!kvp.Equals(default(KeyValuePair<string, string>)) && !string.IsNullOrEmpty(kvp.Value))
            {
                return true;
            }

            return false;
        }

        [AllowAnonymous]
        [HttpOptions]
        [Route("api/deal/")]
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }

        [AllowAnonymous]
        [HttpOptions]
        [Route("api/categories/")]
        public HttpResponseMessage CategoriesOptions()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}
