using AttributeRouting.Web.Http;
using DealsWhat.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using DealsWhat.Models;
using Microsoft.ServiceBus;
using Newtonsoft.Json;

namespace DealsWhat.Controllers
{
    public class CouponOrderlineViewModel
    {
        public string DealOption { get; set; }
        public double RegularPrice { get; set; }

        public string FinePrint { get; set; }

        public DateTime EndTime { get; set; }

        public string DealImageUrl { get; set; }

        public string Id { get; set; }

        public string DealUrl { get; set; }
        public Dictionary<string, string> DealAttributes { get; set; }

        public CouponOrderlineViewModel()
        {
            DealAttributes = new Dictionary<string, string>();
        }
    }

    public class RedemptionApiController : ApiController
    {

        private static Lazy<string> couponTemplate = new Lazy<string>(() =>
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/coupon template.html");
            var template = "";
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                template = reader.ReadToEnd();
            }

            return template;
        });



        [GET("api/redemption/{id}")]
        public HttpResponseMessage GetVoucher(string id)
        {
            var endpoint = ConfigurationManager.AppSettings["WebserviceBaseUrl"] + "api/order/coupon?couponValue=" + id;
            var client = new HttpClient();
            var result = client.GetAsync(endpoint).Result;
            var json = result.Content.ReadAsStringAsync().Result;
            var vm = JsonConvert.DeserializeObject<CouponOrderlineViewModel>(json);

            var base64 = QrCodeHelper.GenerateQrCode(id, 150, 150);

            var couponHtml = couponTemplate.Value.Replace("{{FinePrint}}", vm.FinePrint)
                .Replace("{{SpecialPrice}}", vm.RegularPrice.ToString("0.00"))
                .Replace("{{EndDate}}", vm.EndTime.ToLongDateString())
                .Replace("{{ShortDescription}}", vm.DealOption)
                .Replace("{{DealImage}}", vm.DealImageUrl)
                .Replace("{{LogoImage}}", ConfigurationManager.AppSettings["BaseUrl"] + "Images/dealswhat.png")
                .Replace("{{Barcode}}", base64.ToHtmlString());

            //var basePath = HttpContext.Current.Server.MapPath("~");
            //var fileName = basePath + "\\coupons\\" + "coupon.pdf";
            //using (var stream = new FileStream(fileName, FileMode.Create))
            //{
            //    WkHtmlToPdf.GeneratePdf(basePath + "\\bin\\", couponHtml, stream, new Size(220, 220));
            //}

            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(couponHtml);



            var basePath = HttpContext.Current.Server.MapPath("~");
            //var fileName = basePath + "\\coupons\\" + "coupon.pdf";
            var stream = new MemoryStream();

            //WkHtmlToPdf.GeneratePdf(basePath + "\\bin\\", couponHtml, stream, new Size(220, 220));

            //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            //var pdfBytes = htmlToPdf.GeneratePdf(couponHtml);

            var pdfBytes = GeneratePdf(couponHtml);
            stream.Write(pdfBytes, 0, pdfBytes.Length);
            stream.Position = 0;

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);

            var fileName = GenerateRandomName() + ".pdf";
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return response;

        }

        private static byte[] GeneratePdf(string html)
        {
            var binding = new NetTcpRelayBinding();
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 2147483647;

            var cf = new ChannelFactory<IPdfGeneratorChannel>(
       binding,
       new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", "dw-sea", "pdf")));

            cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior
            { TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "5EefGkLJ6vDO3r2/+B71NWDv4JMKT8wPtoLtPqXW1Uw=") });

            using (var ch = cf.CreateChannel())
            {
                var bytes = ch.GeneratePdfWithHtml(html);

                return bytes;
            }
        }

        private static Random random = new Random();
        private static string GenerateRandomName()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 18)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }


    }
}