CREATE TABLE [dbo].[cookbook] (
	[cookbook_id]	int				IDENTITY								NOT NULL,
	[title]			varchar(100)											NOT NULL,
	PRIMARY KEY(cookbook_id)
);

CREATE TABLE [dbo].[recipe] (
	[recipe_id]		int				IDENTITY								NOT NULL,
	[cookbook_id]	int				REFERENCES cookbook(cookbook_id)				,
	[title]			varchar(100)											NOT NULL,
	[descript]		text													NOT NULL,
	[serving_size]	int																,
	PRIMARY KEY (recipe_id)
);

CREATE TABLE [dbo].[ingredient] (
	[ingredient_id]		int				IDENTITY							NOT NULL,
	[recipe_id]			int				REFERENCES recipe(recipe_id)				,
	[portion]			int													NOT NULL,
	[measurement]		varchar(20)											NOT NULL,
	[item]				varchar(50)													,
	PRIMARY KEY (ingredient_id)
);