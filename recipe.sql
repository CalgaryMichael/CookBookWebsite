CREATE Table recipe (
	recipe_id		int				IDENTITY NOT NULL,
	cookbook_id		int				REFERENCES cookbook(cookbook_id),
	title			varchar(100)	NOT NULL,
	descript		text			NOT NULL,
	serving_size	int				,
	PRIMARY KEY (recipe_id)
)