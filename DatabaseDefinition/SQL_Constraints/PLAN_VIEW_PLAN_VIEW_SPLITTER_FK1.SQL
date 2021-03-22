/* Add foreign key constraints to table "PLAN_VIEW_SPLITTER".                                   */
alter table "PLAN_VIEW_SPLITTER"
	add constraint "PLAN_VIEW_PLAN_VIEW_SPLITTER_FK1" foreign key (
		"VIEW_RID")
	 references "PLAN_VIEW" (
		"VIEW_RID")  

go
