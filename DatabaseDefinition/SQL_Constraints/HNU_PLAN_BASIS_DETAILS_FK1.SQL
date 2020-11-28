alter table "USER_PLAN_BASIS_DETAILS"
	add constraint "HNU_PLAN_BASIS_DETAILS_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
