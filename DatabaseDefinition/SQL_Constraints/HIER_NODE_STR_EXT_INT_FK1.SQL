/* Add foreign key constraints to table "STORE_EXTERNAL_INTRANSIT".                           */
alter table "STORE_EXTERNAL_INTRANSIT"
	add constraint "HIER_NODE_STR_EXT_INT_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
