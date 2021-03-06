﻿using System.Linq;
using System.Web.Mvc;
using DealsWhat.Models;
using DealsWhat.ViewModels;

namespace DealsWhat.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        [HttpPost]
        public ActionResult Add(string dealId)
        {
            //var emailAddress = User.Identity.Name;
            //var dealGuid = default(Guid);

            //Guid.TryParse(dealId, out dealGuid);

            //using (var context = new DealsContext())
            //{
            //    var user = context.Users.First(u => u.EmailAddress == emailAddress);
            //    var deal = context.Deals.First(d => d.Id == dealGuid);
            //    var cart = new Cart
            //    {
            //        Id = Guid.NewGuid(),
            //        Deal = deal,
            //        Quantity = 1,
            //        User = user
            //    };

            //    context.Carts.Add(cart);
            //    context.SaveChanges();
            //}

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Index()
        {
            var cartAdded = Request.QueryString["cartAdded"];

            if (!string.IsNullOrEmpty(cartAdded))
            {
                ViewBag.CartAdded = true;
            }
            else
            {
                ViewBag.CartAdded = false;
            }
            //using (var context = new DealsContext())
            //{
            //    var emailAddress = User.Identity.Name;
            //    var user = context.Users.First(u => u.EmailAddress == emailAddress);
            //    var carts = context.Carts.Where(c => c.User.UserId == user.UserId).ToList();
            //    var viewModel = new ViewShoppingCartViewModel(carts);

            //    return View(viewModel);
            //}

            return View("");
        }
    }
}
