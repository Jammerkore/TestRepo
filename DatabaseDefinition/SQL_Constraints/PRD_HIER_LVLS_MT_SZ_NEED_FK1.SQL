alter table "METHOD_SIZE_NEED"
	add constraint "PRD_HIER_LVLS_MT_SZ_NEED_FK1" foreign key (
		"MERCH_PH_RID",
		"MERCH_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
