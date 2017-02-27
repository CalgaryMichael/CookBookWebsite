using CookBookWebSite.Models;
using CookBookWebSite.Service;
using System.Collections.Generic;
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
			return PartialView("Recipe", r);
		}


		// GET: CookBook/Create/{id}
		public ActionResult Create(int id) {
			var r = new Recipe();
			r.cookbook = db.ReadCookbook(id);
			r.cookbook_id = r.cookbook.cookbook_id;
			r.recipe_id = db.RecipeCount() + 1;
			return View(r);
		}


		// POST: CookBook/Create
		[HttpPost]
		public ActionResult Create(Recipe r) {
			db.Create(r);
			return RedirectToAction("Ingredients", new { id = r.recipe_id });
		}


		// GET: CookBook/Ingredients/{id}
		public ActionResult Ingredients(int id) {
			ViewBag.ID = id;
			return View();
		}

		
		// GET: CookBook/CurrentIngredients
		public ActionResult CurrentIngredients(int id) {
			var r = db.ReadRecipe(id);
			if (r == null) {
				r = db.ReadNewRecipe(id);
				r.ingredients = new List<Ingredient>();
			}
			return PartialView("CurrentIngredients", r);
		}


		// GET: CookBook/Add
		public ActionResult Add(int id) {
			var i = new Ingredient();
			i.recipe_id = id;
			return PartialView("Add", i);
		}


		// POST: CookBook/Add
		[HttpPost]
		public ActionResult Add(Ingredient i) {
			db.Create(i);
			return RedirectToAction("Ingredients", new { id = i.recipe_id });
		}
    }
}