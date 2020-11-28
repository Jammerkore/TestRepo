/* Add foreign key constraints to table "USER_CURRENT_GRID_VIEW".                             */
alter table "USER_CURRENT_GRID_VIEW"
	add constraint "GRID_VIEW_USER_VIEW_FK1" foreign key (
		"VIEW_RID")
	 references "GRID_VIEW" (
		"VIEW_RID")  

go
