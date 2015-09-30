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
using log4net;
using Newtonsoft.Json;

namespace DealsWhat.Application.WebApi.Controllers
{
    public class MerchantController : ApiController
    {
        private readonly IMerchantService merchantService;
        private static readonly ILog logger = LogManager.GetLogger(typeof(MerchantController));

        public MerchantController(IMerchantService merchantService)
        {
            this.merchantService = merchantService;

            OrderlineMappings.CreateCouponMapping();
            OrderlineMappings.CreateMerchantOrderlineMapping();
            OrderlineMappings.CreateOrderlineMapping();
            MerchantMappings.CreateMerchantInfoMapping();
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/merchant/info")]
        public HttpResponseMessage GetMerchantInfo()
        {
            var emailAddress = User.Identity.Name;
            var merchantModel = this.merchantService.GetMerchantInfo(emailAddress);

            if (merchantModel == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var vm = AutoMapper.Mapper.Map<MerchantInfoViewModel>(merchantModel);
            var json = JsonConvert.SerializeObject(vm);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json);

            return response;
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
                logger.InfoFormat("Redeeming coupon value {0}.", model.Value);
                var couponRedemption = new CouponRedemption(model.Value);
                var orderline = this.merchantService.RedeemCoupon(couponRedemption);

                logger.InfoFormat("Successfully redeemed coupon value {0}. Resulting orderline JSON: {1}", model.Value, JsonConvert.SerializeObject(orderline));
                vm = AutoMapper.Mapper.Map<OrderlineModel, OrderlineViewModel>(orderline);
            }
            catch (CouponAlreadyRedeemedException ex)
            {
                logger.WarnFormat("Coupon value {0} is already redeemed.", model.Value);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (CouponNotFoundException ex)
            {
                logger.WarnFormat("Coupon value {0} is not found.", model.Value);
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(vm));

            return response;
        }

        [AllowAnonymous]
        [HttpOptions]
        [Route("api/merchant/info")]
        public HttpResponseMessage MerchantInfoOptions()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
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
}