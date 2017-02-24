using CookBookWebSite.Models;
using CookBookWebSite.Service;
using System.Web.Mvc;

namespace CookBookWebSite.Controllers {
    public class CookBookController : Controller {
		Database db = new Database();

		// GET: CookBook/
		public ActionResult index() {
			var cb = db.ReadCookbook(1);
			return View(cb);
		}


        // GET: CookBook/TestDb
        public ActionResult TestDb() {
			db.initDatabase();
			return RedirectToAction("index");
        }

		
		// GET: CookBook/Recipe/{id}
		public ActionResult Recipe(int id) {
			var r = db.ReadRecipe(id);
			return View(r);
		}
    }
}