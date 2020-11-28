/* Add foreign key constraints to table "DAILY_PERCENTAGES".                                  */
alter table "DAILY_PERCENTAGES"
	add constraint "HIER_NODE_DLY_PCTS_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
