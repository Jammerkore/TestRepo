alter table "HIER_NODE_JOIN"
	add constraint "H_N_HIER_NODE_JOIN_FK2" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
