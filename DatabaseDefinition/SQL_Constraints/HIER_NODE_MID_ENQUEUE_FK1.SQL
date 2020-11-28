/* Add foreign key constraints to table "MID_ENQUEUE".                                        */
alter table "MID_ENQUEUE"
	add constraint "HIER_NODE_MID_ENQUEUE_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
