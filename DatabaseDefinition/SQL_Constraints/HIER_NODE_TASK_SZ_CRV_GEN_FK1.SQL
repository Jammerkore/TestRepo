alter table "TASK_SIZE_CURVE_GENERATE"
	add constraint "HIER_NODE_TASK_SZ_CRV_GEN_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
