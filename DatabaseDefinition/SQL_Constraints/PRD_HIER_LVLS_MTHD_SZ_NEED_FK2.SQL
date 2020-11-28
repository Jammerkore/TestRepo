alter table "METHOD_SIZE_NEED"
	add constraint "PRD_HIER_LVLS_MTHD_SZ_NEED_FK2" foreign key (
		"GENCURVE_PH_RID",
		"GENCURVE_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
