alter table "OTS_PLAN"
	add constraint "HN_METHOD_OTS_PLAN_FK1" foreign key (
		"PLAN_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
