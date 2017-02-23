CREATE Table ingredient (
	ingredient_id		int				IDENTITY						NOT NULL,
	recipe_id			int				REFERENCES recipe(recipe_id)			,
	portion				int												NOT NULL,
	measurement			varchar(20)										NOT NULL,
	item				varchar(50)												,
	PRIMARY KEY (recipe_id)
)