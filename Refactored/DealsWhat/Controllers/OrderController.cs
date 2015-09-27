using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
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
    public class CheckOutToIPayViewModel
    {
        public string MerchantCode { get; set; }
        public string RefNo { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string ProdDesc { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserContact { get; set; }
        public string Remark { get; set; }
        public string Lang { get; set; }
        public string Signature { get; set; }
        public string ResponseURL { get; set; }
        public string BackendURL { get; set; }

        public string PaymentId { get; set; }
    }
    public class OrderViewModel
    {
        public string Id { get; set; }
        public double TotalSpecialPrice { get; set; }
    }
    public class UserInfoViewModel
    {
        public string Email { get; set; }


    }

    public class NewPaymentViewModel
    {
        public string OrderId { get; set; }
    }

    public class OrderController : Controller
    {
        //private string merchantCode = "M07850";
        //private string merchantKey = "X13J179r2K";
        private string merchantCode = "M05178";
        private string merchantKey = "eK0kKeJVRn";

        //private string merchantCode = "M07850";
        //private string merchantKey = "X13J179r2K";
        private string currency = "MYR";
        private string paymentUrl = "http://localhost:13251/api/payment";
        private string newOrderUrl = "http://localhost:39874/api/order/new";
        private string orderPaidUrl = "http://localhost:39874/api/order/paid";
        private string baseUrl = "";

        private string profileUrl = "http://localhost:39874/api/account/userinfo";

        public OrderController()
        {
            var webserviceBaseUrl = ConfigurationManager.AppSettings["WebserviceBaseUrl"];
            newOrderUrl = webserviceBaseUrl + "api/order/new";
            orderPaidUrl = webserviceBaseUrl + "api/order/paid";
            paymentUrl = ConfigurationManager.AppSettings["PaymentUrl"];
            baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            profileUrl = webserviceBaseUrl + "api/account/userinfo";
        }

        public ActionResult CheckOutPayment()
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
        public ActionResult Payment()
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
                var token = "";
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
        public ActionResult CheckOutPayment(NewPaymentViewModel formData)
        {
            var token = Request.Cookies["token"].Value;

            var orderId = formData.OrderId;
            var totalAmount = 0.0;
            var email = "";

            if (string.IsNullOrWhiteSpace(orderId))
            {
                try
                {
                    var profileRequest = WebRequest.Create(profileUrl);
                    profileRequest.Headers.Add("Authorization", "Bearer " + token);
                    profileRequest.Method = "get";
                    profileRequest.ContentLength = 0;

                    var profileResponse = profileRequest.GetResponse();
                    var profileWebResponse = (HttpWebResponse)profileResponse;

                    using (var reader = new StreamReader(profileWebResponse.GetResponseStream()))
                    {
                        var json = reader.ReadToEnd();
                        var profile = JsonConvert.DeserializeObject<UserInfoViewModel>(json);

                        email = profile.Email;
                    }


                    var orderRequest = WebRequest.Create(newOrderUrl);
                    orderRequest.Headers.Add("Authorization", "Bearer " + token);
                    orderRequest.Method = "post";
                    orderRequest.ContentLength = 0;

                    var orderResponse = orderRequest.GetResponse();
                    var newOrderResponse = (HttpWebResponse)orderResponse;

                    if (newOrderResponse.StatusCode == HttpStatusCode.Created)
                    {
                        using (var reader = new StreamReader(newOrderResponse.GetResponseStream()))
                        {
                            var json = reader.ReadToEnd();
                            var order = JsonConvert.DeserializeObject<OrderViewModel>(json);
                            orderId = order.Id;
                            //totalAmount = order.TotalSpecialPrice;
                            totalAmount = 0.50;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.NoBillingAddress = true;
                    return View("CheckOutPayment");
                }
            }

            var productDescription = "Order " + orderId;
            var username = email;

            var phoneNumber = "0162223322";
            //var signature = GenerateSignature("1", totalAmount);

            //var request = (HttpWebRequest)WebRequest.Create(paymentUrl);

            //var postData = "MerchantCode=" + merchantCode;
            //postData += "&PaymentId=";
            //postData += "&RefNo=1";
            //postData += "&Amount=" + totalAmount.ToString("0.00");
            //postData += "&Currency=" + currency;
            //postData += "&ProdDesc=" + productDescription;
            //postData += "&UserName=" + username;
            //postData += "&UserEmail=" + email;
            //postData += "&UserContact=" + phoneNumber;
            //postData += "&Remark=";
            //postData += "&Lang=";
            //postData += "&Signature=" + signature;
            //postData += "&ResponseURL=" + baseUrl + Url.Action("Payment");
            //postData += "&BackendURL=" + baseUrl + Url.Action("Payment");

            //var data = Encoding.ASCII.GetBytes(postData);

            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = data.Length;
            //request.Referer = "http://dealswhat.my/order/checkout";

            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            //var response = request.GetResponse();
        
            //var responseStream = response.GetResponseStream();

            var IpayViewModel = new CheckOutToIPayViewModel
            {
                RefNo = orderId,
                Amount = totalAmount.ToString("0.00"),
                ProdDesc = productDescription,
                UserName =  username,
                UserContact = phoneNumber,
                UserEmail = username
            };

            return RedirectToAction("CheckOut", IpayViewModel);

            //return File(responseStream, response.ContentType);
        }

        public ActionResult CheckOut(CheckOutToIPayViewModel model)
        {
            model.MerchantCode = merchantCode;
            model.PaymentId = "";
            model.Currency = currency;
            model.Remark = "";
            model.Lang = "";
            model.Signature = GenerateSignature(model.RefNo, model.Amount);
            model.ResponseURL = baseUrl + "/Order/Payment";
            model.BackendURL = baseUrl + "/Order/Payment";

            return View(model);
        }

        private string GenerateSignature(
           string refNo,
            string amount)
        {
            var parsedAmount = amount.ToString().Replace(".", "");
            var data = string.Concat(merchantKey, merchantCode, refNo, parsedAmount, currency);
            var sha = GetSHA1HashData(data);

            return sha;
        }

        private string GetSHA1HashData(string data)
        {
            //create new instance of md5
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            //return returnValue.ToString();

            return System.Convert.ToBase64String(hashData);
        }
    }
}
