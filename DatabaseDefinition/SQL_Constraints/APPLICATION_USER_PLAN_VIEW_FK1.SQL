/* Add foreign key constraints to table "PLAN_VIEW".                                          */
alter table "PLAN_VIEW"
	add constraint "APPLICATION_USER_PLAN_VIEW_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
