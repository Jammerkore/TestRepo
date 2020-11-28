alter table "DEFAULT_OTS_PLAN_METHOD"
	add constraint "HN_DEFAULT_METHOD_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
