/* Add foreign key constraints to table "GRID_VIEW_SPLITTER".                                   */
alter table "GRID_VIEW_SPLITTER"
	add constraint "APPLICATION_USER_GRID_VIEW_SPLITTER_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
