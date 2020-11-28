/* Add foreign key constraints to table "STORE_GRADES".                                       */
alter table "STORE_GRADES"
	add constraint "HIER_NODE_STORE_GRADES_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
