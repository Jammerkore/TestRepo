/* Add foreign key constraints to table "HIER_CHAR_GROUP_JOIN".                               */
alter table "HIER_CHAR_GROUP_JOIN"
	add constraint "NODE_HIER_CHAR_GROUP_JOIN_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
