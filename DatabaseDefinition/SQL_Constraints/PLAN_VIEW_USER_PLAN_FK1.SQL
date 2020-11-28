alter table "USER_PLAN"
	add constraint "PLAN_VIEW_USER_PLAN_FK1" foreign key (
		"VIEW_RID")
	 references "PLAN_VIEW" (
		"VIEW_RID")  

go
