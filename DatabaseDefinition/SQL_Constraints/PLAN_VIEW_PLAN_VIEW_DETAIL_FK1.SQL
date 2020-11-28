/* Add foreign key constraints to table "PLAN_VIEW_DETAIL".                                   */
alter table "PLAN_VIEW_DETAIL"
	add constraint "PLAN_VIEW_PLAN_VIEW_DETAIL_FK1" foreign key (
		"VIEW_RID")
	 references "PLAN_VIEW" (
		"VIEW_RID")  

go
