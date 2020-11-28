/* Add foreign key constraints to table "GRID_VIEW_DETAIL".                                   */
alter table "GRID_VIEW_DETAIL"
	add constraint "GRID_VIEW_DETAIL_FK1" foreign key (
		"VIEW_RID")
	 references "GRID_VIEW" (
		"VIEW_RID")  

go
