alter table "USER_PLAN"
	add constraint "HN_USER_PLAN_SELECTED_FK2" foreign key (
		"CHAIN_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
