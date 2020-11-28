/* Add foreign key constraints to table "HIER_CHAR_JOIN".                                     */
alter table "HIER_CHAR_JOIN"
	add constraint "HIER_NODE_HIER_CHAR_JOIN_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
