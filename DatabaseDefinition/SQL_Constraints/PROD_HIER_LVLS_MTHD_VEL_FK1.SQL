alter table "METHOD_VELOCITY"
	add constraint "PROD_HIER_LVLS_MTHD_VEL_FK1" foreign key (
		"OTS_PLAN_PH_RID",
		"OTS_PLAN_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
