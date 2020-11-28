alter table "USER_PLAN"
	add constraint "SG_USER_PLAN_SELECTED_FK1" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
