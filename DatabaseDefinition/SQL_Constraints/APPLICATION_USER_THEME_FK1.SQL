/* Add foreign key constraints to table "THEME".                                              */
alter table "THEME"
	add constraint "APPLICATION_USER_THEME_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
