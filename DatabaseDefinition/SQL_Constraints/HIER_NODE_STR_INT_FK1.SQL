/* Add foreign key constraints to table "STORE_INTRANSIT".                                    */
alter table "STORE_INTRANSIT"
	add constraint "HIER_NODE_STR_INT_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
