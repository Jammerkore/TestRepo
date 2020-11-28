alter table "METHOD_OVERRIDE"
	add constraint "PRD_HIER_LVLS_MTHD_OVRD_FK1" foreign key (
		"MERCH_PH_RID",
		"MERCH_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
