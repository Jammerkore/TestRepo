alter table "TASK_ROLLUP"
	add constraint "PROD_HIER_LVLS_TASK_ROLLUP_FK2" foreign key (
		"TO_PH_RID",
		"TO_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
