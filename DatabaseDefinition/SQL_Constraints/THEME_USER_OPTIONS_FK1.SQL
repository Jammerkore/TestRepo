alter table "USER_OPTIONS"
	add constraint "THEME_USER_OPTIONS_FK1" foreign key (
		"THEME_RID")
	 references "THEME" (
		"THEME_RID")  

go
