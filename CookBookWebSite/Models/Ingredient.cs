namespace CookBookWebSite.Models {
	public class Ingredient {
		// {portion} {measurement} - {item}
		// Exmaple:
		// 12 oz - Steak

		public int ingredient_id { get; set; }
		public int recipe_id { get; set; }
		public int portion { get; set; }
		public string measurement { get; set; }
		public string item { get; set; }

		public Recipe recipe { get; set; }
	}
}