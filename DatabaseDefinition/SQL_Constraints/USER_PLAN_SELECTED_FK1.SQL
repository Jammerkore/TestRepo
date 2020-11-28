/* Add foreign key constraints to table "USER_PLAN".                                          */
alter table "USER_PLAN"
	add constraint "USER_PLAN_SELECTED_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
