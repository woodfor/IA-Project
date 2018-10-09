using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAproject.Models;
namespace IAproject.Controllers
{
    public class CalculationController : Controller
    {
        Calculation cal = new Calculation();
        

        // GET: Calculation
        public ActionResult Index()
        {
            return View();
        }
       
    }
}