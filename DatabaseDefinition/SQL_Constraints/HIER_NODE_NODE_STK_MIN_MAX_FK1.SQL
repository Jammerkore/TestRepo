/* Add foreign key constraints to table "NODE_STOCK_MIN_MAX".                                 */
alter table "NODE_STOCK_MIN_MAX"
	add constraint "HIER_NODE_NODE_STK_MIN_MAX_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
