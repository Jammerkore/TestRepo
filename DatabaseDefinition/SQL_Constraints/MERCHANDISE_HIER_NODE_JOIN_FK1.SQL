/* Add foreign key constraints to table "HIER_NODE_JOIN".                                     */
alter table "HIER_NODE_JOIN"
	add constraint "MERCHANDISE_HIER_NODE_JOIN_FK1" foreign key (
		"PH_RID")
	 references "PRODUCT_HIERARCHY" (
		"PH_RID")  

go
