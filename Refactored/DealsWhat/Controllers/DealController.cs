using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsWhat.Controllers.Aggregator;
using DealsWhat.Helpers;
using DealsWhat.Models;
using DealsWhat.ViewModels;

namespace DealsWhat.Controllers
{
    public class DealController : Controller
    {
        //
        // GET: /Deal/
        public ActionResult Index()
        {
            return View("");
        }

    }
}
