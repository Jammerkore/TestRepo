/* Add foreign key constraints to table "GRID_VIEW".                                          */
alter table "GRID_VIEW"
	add constraint "APPLICATION_USER_GRID_VIEW_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
