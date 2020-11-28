alter table "HEADER_SIZE_NEED"
	add constraint "HIER_LVLS_HDR_SZ_ND_FK1" foreign key (
		"PH_RID",
		"PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
