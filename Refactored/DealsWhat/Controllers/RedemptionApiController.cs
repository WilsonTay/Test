using AttributeRouting.Web.Http;
using DealsWhat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DealsWhat.Controllers
{
    public class RedemptionApiController : ApiController
    {
        [GET("api/redemption/{id}")]
        public HttpResponseMessage GetVoucher(string id)
        {
             
            var base64 = QrCodeHelper.GenerateQrCode(id);

            var content = new StringContent(base64.ToHtmlString());
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = content;

            return response;
        }

        
    }
}