using System.Collections.Generic;
using System.Configuration;
using CookBookWebSite.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using CookBookWebSite.Service.SQL;
using System.Linq;

namespace CookBookWebSite.Service {
	public class Database {
		private string connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
		private string master = ConfigurationManager.ConnectionStrings["MasterConnection"].ConnectionString;

		#region Database Creation

		// Initialize the Database
		public void initDatabase() {
			CreateDatabase();
			CreateTables();
			InsertData();
		}

		// Drop & Create Database
		private void CreateDatabase() {
			using (IDbConnection db = new SqlConnection(master)) {
				db.Execute(Scripts.CreateDatabaseSql);
			}
		}


		// Create Tables to fill the Database
		private void CreateTables() {
			using (IDbConnection db = new SqlConnection(connection)) {
				db.Execute(Scripts.CreateTablesSql);
			}
		}


		// Fill tables with dummy data
		private void InsertData() {
			using (IDbConnection db = new SqlConnection(connection)) {
				db.Execute(Scripts.InsertDummyDataSql);
			}
		}

		#endregion


		#region Create

		// Create new row in CookBook
		public void Create(CookBook c) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "INSERT INTO cookbook VALUES(@title)";
				db.Execute(sqlQuery, c);
			}
		}


		// Create new row in Recipe
		public void Create(Recipe r) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "INSERT INTO recipe VALUES(@recipe_id, @cookbook_id, @title, @descript, @serving_size)";
				db.Execute(sqlQuery, r);
			}
		}


		// Create new row in Ingredient
		public void Create(Ingredient i) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "INSERT INTO ingredient VALUES(@recipe_id, @portion, @measurement, @item)";
				db.Execute(sqlQuery, i);
			}
		}

		#endregion


		#region Read
		
		// Populate Single Core with all Competencies tied to it
		public Recipe ReadRecipe(int ID) {
			Recipe recipe = null;

			using (IDbConnection db = new SqlConnection(connection)) {
				string sql = @"SELECT c.*, r.*, i.*
								FROM cookbook AS c, recipe AS r, ingredient AS i
								WHERE i.recipe_id = r.recipe_id
								AND c.cookbook_id = r.cookbook_id
								AND r.recipe_id = @id";
				var result = db.Query<CookBook, Recipe, Ingredient, Ingredient>(sql,
					(c, r, i) => {
						if (recipe == null) {
							recipe = r;
							recipe.cookbook = c;
						}

						i.recipe = r;
						return i;
					}, new { id = ID },
					splitOn: "recipe_id,ingredient_id").AsList();
				if (recipe != null) {
					recipe.ingredients = result;
				}
			}

			return recipe;
		}


		public Recipe ReadNewRecipe(int ID) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sql = @"SELECT c.*, r.*
								FROM cookbook AS c, recipe AS r
								WHERE c.cookbook_id = r.cookbook_id
								AND r.recipe_id = @id";
				return db.Query<Recipe>(sql, new { id = ID }).SingleOrDefault();
			}
		}


		// Populate Single Core with all Competencies tied to it
		public CookBook ReadCookbook(int ID) {
			CookBook cookbook = null;
			var lookup = new Dictionary<int, Recipe>();

			using (IDbConnection db = new SqlConnection(connection)) {
				string sql = @"SELECT c.*, r.*, i.*
								FROM cookbook AS c, recipe AS r, ingredient AS i
								WHERE i.recipe_id = r.recipe_id
								AND r.cookbook_id = c.cookbook_id
								AND c.cookbook_id = @id";
				var result = db.Query<CookBook, Recipe, Ingredient, Recipe>(sql,
					(c, r, i) => {
						// make sure that CookBook is not null
						if (cookbook == null) {
							cookbook = c;
							c.recipes = new List<Recipe>();
						}

						// check to see if 'r' is new or if we already have
						// that recipe in our list
						Recipe recipe;
						if (!lookup.TryGetValue(r.recipe_id, out recipe)) {
							lookup.Add(r.recipe_id, recipe = r);
						}

						// make sure that recipe knows what CookBook it is a part of
						if (recipe.cookbook == null)
							recipe.cookbook = c;

						// make sure that ingredient knows what recipe it is a part of
						if (i.recipe == null)
							i.recipe = recipe;

						// add ingredient to recipe
						if (recipe.ingredients == null)
							recipe.ingredients = new List<Ingredient>();
						recipe.ingredients.Add(i);

						return recipe;
					}, new { id = ID },
					splitOn: "cookbook_id,recipe_id,ingredient_id").AsList();
				if (cookbook != null) {
					cookbook.recipes = lookup.Values.ToList();
				}
			}

			return cookbook;
		}


		#endregion


		#region Update

		// Update row in CookBook
		public void Update(CookBook c) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "UPDATE cookbook SET title = @title WHERE cookbook_id = @cookbook_id";
				db.Execute(sqlQuery, c);
			}
		}


		// Update row in Recipe
		public void Update(Recipe r) {
			using (IDbConnection db = new SqlConnection(connection)) {
				var sqlQuery = "UPDATE Recipe SET cookbook_id = @cookbook_id, title = @title, descript = @decsript, serving_size = @serving_size WHERE recipe_id = @recipe_id";
				db.Execute(sqlQuery, r);
			}
		}


		// Update row in Ingredient
		public void Update(Ingredient i) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "UPDATE ingredient SET recipe_id = @recipe_id, portion = @portion, measurement = @measurement, item = @item WHERE ingredient_id = @ingredient_id";
				db.Execute(sqlQuery, i);
			}
		}

		#endregion


		#region Delete

		// Delete row in CookBook
		public void Delete(CookBook c) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "DELETE cookbook WHERE cookbook_id = @cookbook_id";
				db.Execute(sqlQuery, c);
			}
		}


		// Delete row in Recipe
		public void Delete(Recipe r) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "DELETE recipe WHERE recipe_id = @recipe_id";
				db.Execute(sqlQuery, r);
			}
		}


		// Delete row in Ingredient
		public void Delete(Ingredient i) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "DELETE ingredient WHERE ingredient_id = @ingredient_id";
				db.Execute(sqlQuery, i);
			}
		}

		#endregion


		#region Misc

		public int RecipeCount() {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sql = "SELECT COUNT(*) FROM recipe";
				return db.Query<int>(sql).Single();
			}
		}

		#endregion
	}
}