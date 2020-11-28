/* Add foreign key constraints to table "STORE_CAPACITY".                                     */
alter table "STORE_CAPACITY"
	add constraint "HIER_NODE_STR_CAPACITY_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
