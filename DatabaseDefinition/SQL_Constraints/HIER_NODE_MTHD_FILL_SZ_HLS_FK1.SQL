alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "HIER_NODE_MTHD_FILL_SZ_HLS_FK1" foreign key (
		"MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
