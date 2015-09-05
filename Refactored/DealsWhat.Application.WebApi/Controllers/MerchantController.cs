using DealsWhat.Application.WebApi.Mappings;
using DealsWhat.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DealsWhat.Application.WebApi.Controllers
{
    public class MerchantOrderlineViewModel
    {
         public IList<string> AddressLines { get; set; }

        public DateTime DatePlaced { get; set; }

        public double SpecialPrice { get; set; }
        public double RegularPrice { get; set; }

        public string DealOption { get; set; }

        public string DealUrl { get; set; }

        public string DealThumbnailUrl { get; set; }

        public Dictionary<string, string> DealAttributes { get; set; }

        public int Quantity { get; set; }

        public string Id { get; set; }

        public MerchantOrderlineViewModel()
        {
            DealAttributes = new Dictionary<string, string>();
            AddressLines = new List<string>();
        }
    }
    public class MerchantController : ApiController
    {
        IMerchantService merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            this.merchantService = merchantService;

            OrderlineMappings.CreateMerchantOrderlineMapping();
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

            searchQuery.MerchantId = merchantId.Value;

            return this.merchantService.SearchOrderlines(searchQuery).Select(o => AutoMapper.Mapper.Map<MerchantOrderlineViewModel>(o));
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