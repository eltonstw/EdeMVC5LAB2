using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LAB.Controllers
{
    public class ViewTestController : Controller
    {
        // GET: ViewTest
        public ActionResult Index()
        {
            return View(new int[] {1,2,3,4,5});
        }
    }
}