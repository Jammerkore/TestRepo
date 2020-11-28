alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "PRD_HIER_LVLS_MT_FL_SZ_HLS_FK1" foreign key (
		"MERCH_PH_RID",
		"MERCH_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
