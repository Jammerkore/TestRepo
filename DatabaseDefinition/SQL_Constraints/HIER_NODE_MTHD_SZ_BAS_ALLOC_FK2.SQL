alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "HIER_NODE_MTHD_SZ_BAS_ALLOC_FK2" foreign key (
		"GENCURVE_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
