alter table "HIER_NODE_JOIN"
	add constraint "HIER_NODE_HIER_NODE_JOIN_FK1" foreign key (
		"PARENT_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
