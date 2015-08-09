using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using DealsWhat.Domain.Interfaces;

namespace DealsWhat.Application.WebApi.Controllers
{
    public class PaymentController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private string merchantCode = "M07850";
        private string merchantKey = "X13J179r2K";
        private string currency = "MYR";

        public PaymentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("api/payment/")]
        public HttpResponseMessage Get(string id)
        {
            var orderRepository = unitOfWork.CreateOrderRepository();
            var order = orderRepository.FindByKey(id);

            order.SetOrderPaid();
            orderRepository.Save();

            return Request.CreateResponse();
        }

        [Route("api/payment/complete")]
        public HttpResponseMessage PaymentComplete(string orderId)
        {
            var orderRepository = unitOfWork.CreateOrderRepository();
            var order = orderRepository.FindByKey(orderId);

            return Request.CreateResponse();
        }


        //[Route("api/payment")]
        //public HttpResponseMessage Post()
        //{
        //    var orderId = "";
        //    var orderRepository = repositoryFactory.CreateOrderRepository();
        //    var userRepository = repositoryFactory.CreateUserRepository();

        //    var email = User.Identity.Name;
        //    var order = orderRepository.FindByKey(orderId);
        //    var user = userRepository.FindByEmailAddress(email);

        //    var amount = 0.0;
        //    foreach (var orderLine in order.Orderlines)
        //    {
        //        amount += orderLine.DealOption.SpecialPrice;
        //    }

        //    var paymentId = "";
        //    var referenceNumber = order.Key;
        //    var productDescription = string.Format("Product {0}", order.Key);
        //    var username = user.EmailAddress;

        //    var request = WebRequest.Create("http://www.ragezone.com");

        //    var postData = "MerchantCode=" + merchantCode;
        //    postData += "&PaymentId=";
        //    postData += "&RefNo=" + orderId;
        //    postData += "&Amount=" + amount;
        //    postData += "&Currency=" + currency;
        //    postData += "&ProdDesc=" + productDescription;
        //    postData += "&UserName=" + username;
        //    postData += "&UserEmail=" + email;
        //    //postData += "&UserContact=" + phoneNumber;
        //    //postData += "&Remark=";
        //    //postData += "&Lang=";
        //    //postData += "&Signature=" + signature;
        //    //postData += "&ReponseURL=" + responseUrl;
        //    //postData += "&BackendURL" + backendUrl;

        //    var data = Encoding.ASCII.GetBytes(postData);

        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = data.Length;

        //    using (var stream = request.GetRequestStream())
        //    {
        //        stream.Write(data, 0, data.Length);
        //    }


        //    var responseStream = request.GetResponse().GetResponseStream();

        //    var response = new HttpResponseMessage();
        //    response.Content = new StreamContent(responseStream);
        //    return response;
        //}



        public string Hash(byte[] temp)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(temp);
                return Convert.ToBase64String(hash);
            }
        }

    }
}
