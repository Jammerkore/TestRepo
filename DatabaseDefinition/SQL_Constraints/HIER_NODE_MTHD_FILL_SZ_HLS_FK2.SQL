alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "HIER_NODE_MTHD_FILL_SZ_HLS_FK2" foreign key (
		"GENCURVE_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
