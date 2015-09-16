using DealsWhat.Application.WebApi.Mappings;
using DealsWhat.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Model.Exceptions;
using Newtonsoft.Json;

namespace DealsWhat.Application.WebApi.Controllers
{
    public class MerchantController : ApiController
    {
        IMerchantService merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            this.merchantService = merchantService;

            OrderlineMappings.CreateCouponMapping();
            OrderlineMappings.CreateMerchantOrderlineMapping();
            OrderlineMappings.CreateOrderlineMapping();
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("api/merchant/orderlines/")]
        public IEnumerable<MerchantOrderlineViewModel> GetOrderlines()
        {
            var query = Request.GetQueryNameValuePairs().ToList();
            var searchQuery = new MerchantOrderLineSearchQuery();

            var merchantId = query.FirstOrDefault(k => k.Key.Equals("merchantId", StringComparison.OrdinalIgnoreCase));
            var dealId = query.FirstOrDefault(k => k.Key.Equals("dealId", StringComparison.OrdinalIgnoreCase));

            searchQuery.MerchantId = merchantId.Value;
            searchQuery.DealId = dealId.Value;

            return this.merchantService.SearchOrderlines(searchQuery).Select(o => AutoMapper.Mapper.Map<MerchantOrderlineViewModel>(o));
        }

        [HttpPost]
        [Route("api/merchant/redeem/")]
        public HttpResponseMessage Post([FromBody] RedemptionViewModel model)
        {
            OrderlineViewModel vm = null;

            try
            {
                var couponRedemption = new CouponRedemption(model.Value);
                var orderline = this.merchantService.RedeemCoupon(couponRedemption);
                vm = AutoMapper.Mapper.Map<OrderlineModel, OrderlineViewModel>(orderline);
            }
            catch (CouponAlreadyRedeemedException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(vm));

            return response;
        }

        [AllowAnonymous]
        [HttpOptions]
        [Route("api/merchant/redeem/")]
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public class RedemptionViewModel
    {
        public string Value { get; set; }
    }
}