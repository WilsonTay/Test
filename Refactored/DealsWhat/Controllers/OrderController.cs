using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using DealsWhat.Models;
using DealsWhat.ViewModels;
using Newtonsoft.Json;

namespace DealsWhat.Controllers
{
    public class OrderViewModel
    {
        public string Id { get; set; }
    }

    public class NewPaymentViewModel
    {
        public string OrderId { get; set; }
    }

    public class OrderController : Controller
    {
        private string merchantCode = "M07850";
        private string merchantKey = "X13J179r2K";
        private string currency = "MYR";
        private string paymentUrl = "http://localhost:13251/api/payment";
        private string newOrderUrl = "http://localhost:39874/api/order/new";
        private string orderPaidUrl = "http://localhost:39874/api/order/paid";

        public ActionResult CheckOut()
        {
            //using (var context = new DealsContext())
            //{
            //    var emailAddress = User.Identity.Name;
            //    var user = context.Users.First(u => u.EmailAddress == emailAddress);
            //    var carts = context.Carts.Where(c => c.User.UserId == user.UserId).ToList();
            //    var viewModel = new OrderCheckoutViewModel(carts);

            //    return View(viewModel);
            //}

            return View("");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult PaymentResponseUrl()
        {
            var merchantCode = Request["MerchantCode"];
            var paymentId = Request["PaymentId"];
            var refNo = Request["RefNo"];

            var amount = Request["Amount"];
            var currency = Request["Currency"];
            var signature = Request["Signature"];

            var remark = Request["Remark"];
            var transId = Request["TransId"];
            var authCode = Request["AuthCode"];

            var status = Request["Status"];
            var errDesc = Request["ErrDesc"];

            if (status.Equals("1"))
            {
                var token =
             "kjXTyfBMDI-Op2TjNwnaVzJT7cGQxxNhZo9Jm0bUGkdHc6S--AD32ZLkdj3P18g8xUM6ZIZYmyW1r5ehg4ZXPlYx782l5ylVyD6bR-JiTsZoeP3HoYA4nrfSPjlYoimvWGy7Me2XyJGd4uDw2ooO5ljPxaKByN91xXQ1L5B2Qhm2gmvnsvnIFTvhaEgpLYppKOTIuR1tNFkVs_aVv3H_gAPSJBG-rZJUFeDPq49790GZqTggu_kn3eheo6zrD5ISFr46ErqCP-hYtOsLGCEW_1UenImG9ukJ6fQXjtP4GwHd6kEj_hEVkfV4CoIJut0RUlABGnu1w3B9JSURFnhBUxj7Urr6kCU-KLVyc2uCMJoeuq88cPk1UX5dw2cIbvnDqDA_pgPF8FXjXKzCItyxF94nvAfKwxZLgShn55bclamWAFkBItVNENbwTPCMVFnsS2K3PcCEySUM1RwwWZyXh7SCYUW2Rz4OJGUFSfdLiqjDtTVxXuH10emklveiul0A";
                var orderId = refNo;

                var orderRequest = WebRequest.Create(orderPaidUrl + "?id=" + orderId);
                orderRequest.Headers.Add("Authorization", "Bearer " + token);
                orderRequest.Method = "post";
                orderRequest.ContentLength = 0;
                var newOrderResponse = (HttpWebResponse)orderRequest.GetResponse();

                // TODO: Set order paid.
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                return null;
            }
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult PaymentSuccess()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CheckOutPost(NewPaymentViewModel formData)
        {
            var token =
                "kjXTyfBMDI-Op2TjNwnaVzJT7cGQxxNhZo9Jm0bUGkdHc6S--AD32ZLkdj3P18g8xUM6ZIZYmyW1r5ehg4ZXPlYx782l5ylVyD6bR-JiTsZoeP3HoYA4nrfSPjlYoimvWGy7Me2XyJGd4uDw2ooO5ljPxaKByN91xXQ1L5B2Qhm2gmvnsvnIFTvhaEgpLYppKOTIuR1tNFkVs_aVv3H_gAPSJBG-rZJUFeDPq49790GZqTggu_kn3eheo6zrD5ISFr46ErqCP-hYtOsLGCEW_1UenImG9ukJ6fQXjtP4GwHd6kEj_hEVkfV4CoIJut0RUlABGnu1w3B9JSURFnhBUxj7Urr6kCU-KLVyc2uCMJoeuq88cPk1UX5dw2cIbvnDqDA_pgPF8FXjXKzCItyxF94nvAfKwxZLgShn55bclamWAFkBItVNENbwTPCMVFnsS2K3PcCEySUM1RwwWZyXh7SCYUW2Rz4OJGUFSfdLiqjDtTVxXuH10emklveiul0A";
            var orderId = formData.OrderId;

            if (string.IsNullOrWhiteSpace(orderId))
            {
                var orderRequest = WebRequest.Create(newOrderUrl);
                orderRequest.Headers.Add("Authorization", "Bearer " + token);
                orderRequest.Method = "post";
                orderRequest.ContentLength = 0;
                var newOrderResponse = (HttpWebResponse)orderRequest.GetResponse();

                if (newOrderResponse.StatusCode == HttpStatusCode.Created)
                {
                    using (var reader = new StreamReader(newOrderResponse.GetResponseStream()))
                    {
                        var json = reader.ReadToEnd();
                        var order = JsonConvert.DeserializeObject<OrderViewModel>(json);
                        orderId = order.Id;
                    }
                }
            }

            var amount = 20;
            var productDescription = "Order " + orderId;
            var username = "a@a.com";
            var email = username;
            var phoneNumber = "0162223322";
            var signature = "sig";

            var request = WebRequest.Create(paymentUrl);

            var postData = "MerchantCode=" + merchantCode;
            postData += "&PaymentId=";
            postData += "&RefNo=" + orderId;
            postData += "&Amount=" + amount;
            postData += "&Currency=" + currency;
            postData += "&ProdDesc=" + productDescription;
            postData += "&UserName=" + username;
            postData += "&UserEmail=" + email;
            postData += "&UserContact=" + phoneNumber;
            postData += "&Remark=";
            postData += "&Lang=";
            postData += "&Signature=" + signature;
            postData += "&ResponseURL=" + "http://localhost:1441/Order/PaymentResponseUrl";
            postData += "&BackendURL" + "http://localhost:1441/Order/PaymentResponseUrl";

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            return File(responseStream, response.ContentType);
        }
    }
}
