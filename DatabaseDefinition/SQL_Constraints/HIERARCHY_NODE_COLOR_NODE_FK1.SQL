/* Add foreign key constraints to table "COLOR_NODE".                                         */
alter table "COLOR_NODE"
	add constraint "HIERARCHY_NODE_COLOR_NODE_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
