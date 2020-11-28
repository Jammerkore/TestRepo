/* Add foreign key constraints to table "PLAN_VIEW_FORMAT".                                   */
alter table "PLAN_VIEW_FORMAT"
	add constraint "PLAN_VIEW_PLAN_VIEW_FORMAT_FK1" foreign key (
		"VIEW_RID")
	 references "PLAN_VIEW" (
		"VIEW_RID")  

go
