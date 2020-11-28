/* Add foreign key constraints to table "PRODUCT_HIERARCHY_LEVELS".                           */
alter table "PRODUCT_HIERARCHY_LEVELS"
	add constraint "MERCHANDISE_HIER_LEVELS_FK1" foreign key (
		"PH_RID")
	 references "PRODUCT_HIERARCHY" (
		"PH_RID")  

go
