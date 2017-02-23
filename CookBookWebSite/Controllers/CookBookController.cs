using CookBookWebSite.Data;
using CookBookWebSite.Models;
using CookBookWebSite.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CookBookWebSite.Controllers {
    public class CookBookController : Controller {
		// GET: CookBook/
		public ActionResult index() {
			return View();
		}

        // GET: CookBook/TestDb
        public ActionResult TestDb() {
			Database db = new Database();
			db.initDatabase();
			return View();
        }

		
		// GET: CookBook/Recipe
		public ActionResult Recipe(Recipe r) {
			return View();
		}
    }
}