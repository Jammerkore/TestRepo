/* Add foreign key constraints to table "GRID_VIEW_SPLITTER".                                   */
alter table "GRID_VIEW_SPLITTER"
	add constraint "GRID_VIEW_SPLITTER_FK1" foreign key (
		"VIEW_RID")
	 references "GRID_VIEW" (
		"VIEW_RID")  

go
