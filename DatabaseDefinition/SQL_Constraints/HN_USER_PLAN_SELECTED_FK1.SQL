alter table "USER_PLAN"
	add constraint "HN_USER_PLAN_SELECTED_FK1" foreign key (
		"STORE_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
