alter table "METHOD_SIZE_NEED"
	add constraint "HIER_NODE_MTHD_SZ_NEED_FK3" foreign key (
		"GENCONSTRAINT_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
