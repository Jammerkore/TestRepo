alter table "HEADER_SIZE_NEED"
	add constraint "HIER_ND_HDR_SZ_ND_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
