/* Add foreign key constraints to table "SIZE_NODE".                                          */
alter table "SIZE_NODE"
	add constraint "HIERARCHY_NODE_SIZE_NODE_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
