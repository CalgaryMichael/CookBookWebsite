using System.Collections.Generic;


namespace CookBookWebSite.Models {
	public class Recipe {
		public int recipe_id { get; set; }
		public int cookbook_id { get; set; }
		public string title { get; set; }
		public string descript { get; set; }
		public int serving_size { get; set; }
		public List<Ingredient> ingredients { get; set; }
		public CookBook cookbook { get; set; }
	}
}