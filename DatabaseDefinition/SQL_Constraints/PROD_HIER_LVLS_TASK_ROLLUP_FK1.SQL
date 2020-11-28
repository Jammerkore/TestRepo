alter table "TASK_ROLLUP"
	add constraint "PROD_HIER_LVLS_TASK_ROLLUP_FK1" foreign key (
		"FROM_PH_RID",
		"FROM_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
