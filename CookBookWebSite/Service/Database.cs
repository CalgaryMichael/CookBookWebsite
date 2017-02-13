using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CookBookWebSite.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace CookBookWebSite.Service {
	public class Database {
		private string connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

		#region Database Creation

		// Drop & Create Database
		public void CreateDatabase() {
			using (IDbConnection db = new SqlConnection(connection)) {
				db.Execute(Scripts.CreateDatabaseSql);
			}
		}


		// Create Tables to fill the Database
		public void CreateTables() {
			using (IDbConnection db = new SqlConnection(connection)) {
				db.Execute(Scripts.CreateTablesSql);
			}
		}


		// Fill tables with dummy data
		public void InsertData() {
			using (IDbConnection db = new SqlConnection(connection)) {
				db.Execute(Scripts.InsertDummyDataSql);
			}
		}

		#endregion


		#region Create

		// Create new row in CookBook
		public void Create(CookBook c) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "INSERT INTO cookbook VALUES(@cookbook_id, @title)";
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
				string sqlQuery = "INSERT INTO ingredient VALUES(@ingredient_id, @recipe_id, @portion, @measurement, @item)";
				db.Execute(sqlQuery, i);
			}
		}

		#endregion


		#region Read Multiple
		
		// Read all recipes in a CookBook
		public CookBook ReadAllRecipeForCookBook(int id) {
			CookBook cookbook = null;
			var lookup = new Dictionary<int, Recipe>();

			using (IDbConnection db = new SqlConnection(connection)) {
				string sql = @"SELECT c.*, r.*, i.*,
								FROM cookbook c, recipe as r, ingredient as i
								WHERE i.recipe_id = r.recipe_id
								AND r.cookbook_id = c.cookbook_id
								AND c.cookbook_id = @id";
				db.Query<CookBook, Recipe, Ingredient, CookBook>(sql,
					(c, r, i) => {
						cookbook = c;
						Recipe recipe;

						if (!lookup.TryGetValue(r.recipe_id, out recipe)) {
							recipe = r;
							lookup.Add(r.recipe_id, recipe);
						}

						if (cookbook.recipes == null)
							cookbook.recipes = new List<Recipe>();

						if (recipe.ingredients == null)
							recipe.ingredients = new List<Ingredient>();

						recipe.ingredients.Add(i);
						cookbook.recipes.Add(recipe);
						return cookbook;

					}, splitOn: "cookbook_id,recipe_id,recipe_id");
			}

			return cookbook;
		}


		#endregion


		#region Read Single

		// Populate Single Core with all Competencies tied to it
		public Recipe ReadRecipe(int ID) {
			Recipe recipe = null;

			using (IDbConnection db = new SqlConnection(connection)) {
				string sql = @"SELECT c.*, r.*, i.*,
								FROM recipe as r, ingredient as i
								WHERE i.recipe_id = r.recipe_id
								AND c.cookbook_id = r.cookbook_id
								AND r.recipe_id = @id";
				var result = db.Query<Recipe, CookBook, Ingredient, Ingredient>(sql,
					(r, c, i) => {
						if (recipe == null)
							recipe = r;

						recipe.cookbook = c;
						return i;
					}, new { id = ID },
					splitOn: "recipe_id,recipe_id").AsList();
				if (recipe != null) {
					recipe.ingredients = result;
				}
			}

			return recipe;
		}


		// Populate Single Core with all Competencies tied to it
		public CookBook ReadCookbook(int ID) {
			CookBook cookbook = null;

			using (IDbConnection db = new SqlConnection(connection)) {
				string sql = @"SELECT c.* r.*, i.*,
								FROM cookbook as c, recipe as r, ingredient as i
								WHERE i.recipe_id = r.recipe_id
								AND r.cookbook_id = c.cookbook_id
								AND c.cookbook_id = @id";
				var result = db.Query<CookBook, Recipe, Ingredient, Recipe>(sql,
					(c, r, i) => {
						if (cookbook == null)
							cookbook = c;

						if (r.ingredients == null)
							r.ingredients = new List<Ingredient>();
						r.ingredients.Add(i);

						if (r.cookbook == null)
							r.cookbook = cookbook;

						return r;
					}, new { id = ID },
					splitOn: "cookbook_id,recipe_id,recipe_id").AsList();
				if (cookbook != null) {
					cookbook.recipes = result;
				}
			}

			return cookbook;
		}

		#endregion


		#region Update

		// Update row in Person
		public void Update(CookBook c) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "UPDATE cookbook SET title = @title WHERE cookbook_id = @cookbook_id";
				db.Execute(sqlQuery, c);
			}
		}


		// Update row in Badge
		public void Update(Recipe r) {
			using (IDbConnection db = new SqlConnection(connection)) {
				var sqlQuery = "UPDATE Recipe SET cookbook_id = @cookbook_id, title = @title, descript = @decsript, serving_size = @serving_size WHERE recipe_id = @recipe_id";
				db.Execute(sqlQuery, r);
			}
		}


		// Update row in BadgeReceived
		public void Update(Ingredient i) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "UPDATE ingredient SET recipe_id = @recipe_id, portion = @portion, measurement = @measurement, item = @item WHERE ingredient_id = @ingredient_id";
				db.Execute(sqlQuery, i);
			}
		}

		#endregion


		#region Delete

		// Delete row in Person
		public void Delete(CookBook c) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "DELETE cookbook WHERE cookbook_id = @cookbook_id";
				db.Execute(sqlQuery, c);
			}
		}


		// Delete row in Badge
		public void Delete(Recipe r) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "DELETE recipe WHERE recipe_id = @recipe_id";
				db.Execute(sqlQuery, r);
			}
		}


		// Delete row in BadgeRecieved
		public void Delete(Ingredient i) {
			using (IDbConnection db = new SqlConnection(connection)) {
				string sqlQuery = "DELETE ingredient WHERE ingredient_id = @ingredient_id";
				db.Execute(sqlQuery, i);
			}
		}

		#endregion
	}
}