/* Add foreign key constraints to table "USER_ALLOCATION".                                    */
alter table "USER_ALLOCATION"
	add constraint "HIERARCHY_NODE_USER_ALLOC_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
