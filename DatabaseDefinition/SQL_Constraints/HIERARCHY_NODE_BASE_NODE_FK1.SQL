/* Add foreign key constraints to table "BASE_NODE".                                          */
alter table "BASE_NODE"
	add constraint "HIERARCHY_NODE_BASE_NODE_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
