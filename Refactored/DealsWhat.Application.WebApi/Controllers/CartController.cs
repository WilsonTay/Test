using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac.Core;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Interfaces.Helpers;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Services;

namespace DealsWhat.Application.WebApi.Controllers
{
    [Authorize]
    public class CartController : ApiController
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;

            AutoMapper.Mapper.CreateMap<NewCartItemViewModel, NewCartItemModel>()
                .AfterMap((dest, src) =>
                {
                    if (src.SelectedAttributes.Any())
                    {
                        return;
                    }

                    foreach (var attr in dest.SelectedAttributes)
                    {
                        src.AddSelectedAttributeId(attr);
                    }
                });

            AutoMapper.Mapper.CreateMap<DealAttributeModel, CartItemAttribute>();

            AutoMapper.Mapper.CreateMap<CartItemModel, CartItemViewModel>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.ToString()))
               .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.DealOption.ShortTitle))
               .ForMember(dest => dest.RegularPrice, opt => opt.MapFrom(src => src.DealOption.RegularPrice))
               .ForMember(dest => dest.SpecialPrice, opt => opt.MapFrom(src => src.DealOption.SpecialPrice))
               .ForMember(dest => dest.DealId, opt => opt.MapFrom(src => src.Deal.Key))
               .ForMember(dest => dest.DealUrl, opt => opt.MapFrom(src => src.Deal.CanonicalUrl))
               .AfterMap((dest, src) =>
               {
                   if (src.Attributes.Any())
                   {
                       return;
                   }

                   foreach (var option in dest.AttributeValues)
                   {
                       var converted = AutoMapper.Mapper.Map<CartItemAttribute>(option);
                       src.Attributes.Add(converted);
                   }

                   // TODO: A method for cover photo.
                   src.DealThumbnailUrl = PathHelper.ConvertRelativeToAbsoluteDealImagePath(
                       dest.Deal.Images.OrderBy(a => a.Order).First().RelativeUrl);
               });
        }

        // GET api/<controller>
        public IEnumerable<CartItemViewModel> Get()
        {
            var emailAddress = User.Identity.Name;

            var cartItems = cartService.GetCartItems(emailAddress)
                .Select(item => AutoMapper.Mapper.Map<CartItemViewModel>(item));

            return cartItems;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]NewCartItemViewModel value)
        {
            var emailAddress = User.Identity.Name;
            var newCartModel = AutoMapper.Mapper.Map<NewCartItemModel>(value);

            cartService.AddCartItem(emailAddress, newCartModel);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            var emailAddress = User.Identity.Name;

            cartService.RemoveCartItem(emailAddress, id);
        }

        [AllowAnonymous]
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}