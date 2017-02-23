using CookBookWebSite.Models;
using CookBookWebSite.Service;

namespace CookBookWebSite.Data {
	public class TestData {
		private static Database db;

		public static void populate_database() {
			db = new Database();

			CookBook cb1 = create_cookbook(1);
			create_recipes(cb1, 1);
		}


		private static CookBook create_cookbook(int num) {
			CookBook cookbook = new CookBook();
			cookbook.cookbook_id = num;
			cookbook.title = "Test Cookbook " + cookbook.cookbook_id;
			db.Create(cookbook);
			return cookbook;
		}


		private static void create_recipes(CookBook c, int recipeNum) {
			Recipe r = new Recipe();
			r.recipe_id = recipeNum;
			r.title = "Test Recipe " + (c.recipes.Count + 1);
			r.descript = "Test Description";
			r.serving_size = 5;
			r.cookbook = c;
			r.cookbook_id = c.cookbook_id;
			db.Create(r);

			create_ingredients(r, 1);
			create_ingredients(r, 2);
			create_ingredients(r, 3);

			c.recipes.Add(r);
		}

		
		private static void create_ingredients(Recipe r, int ingredientNum) {
			Ingredient i = new Ingredient();
			i.ingredient_id = ingredientNum;
			i.portion = 12;
			i.measurement = "oz";
			i.item = "Test Ingredient " + r.recipe_id + "." + ingredientNum;
			i.recipe_id = r.recipe_id;
			i.recipe = r;
			r.ingredients.Add(i);

			db.Create(i);
		}
	}
}