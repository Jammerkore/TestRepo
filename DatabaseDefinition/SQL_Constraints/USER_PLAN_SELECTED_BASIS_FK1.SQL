/* Add foreign key constraints to table "USER_PLAN_BASIS".                                    */
alter table "USER_PLAN_BASIS"
	add constraint "USER_PLAN_SELECTED_BASIS_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
