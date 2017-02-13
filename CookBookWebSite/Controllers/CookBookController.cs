using CookBookWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CookBookWebSite.Controllers {
    public class CookBookController : Controller {
        // GET: CookBook
        public ActionResult Index() {
            return View();
        }

		
		// GET: Recipe
		public ActionResult Recipe(Recipe r) {
			return View();
		}
    }
}