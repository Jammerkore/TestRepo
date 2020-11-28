alter table "METHOD_VELOCITY_BASIS"
	add constraint "PRD_HIER_LVLS_MT_VEL_BAS_FK1" foreign key (
		"BASIS_PH_RID",
		"BASIS_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
