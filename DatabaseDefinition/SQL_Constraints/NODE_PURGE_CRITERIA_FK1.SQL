/* Add foreign key constraints to table "PURGE_CRITERIA".                                     */
alter table "PURGE_CRITERIA"
	add constraint "NODE_PURGE_CRITERIA_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
