using System.Collections.Generic;

namespace CookBookWebSite.Models {
	public class CookBook {
		public int cookbook_id { get; set; }
		public string title { get; set; }

		public List<Recipe> recipes { get; set; }
	}
}