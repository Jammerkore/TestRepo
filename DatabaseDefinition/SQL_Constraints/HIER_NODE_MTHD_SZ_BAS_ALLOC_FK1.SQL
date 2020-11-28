alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "HIER_NODE_MTHD_SZ_BAS_ALLOC_FK1" foreign key (
		"GENCONSTRAINT_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
