alter table "METHOD_OVERRIDE"
	add constraint "PRD_HIER_LVLS_MTHD_OVRD_FK2" foreign key (
		"ON_HAND_PH_RID",
		"ON_HAND_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
