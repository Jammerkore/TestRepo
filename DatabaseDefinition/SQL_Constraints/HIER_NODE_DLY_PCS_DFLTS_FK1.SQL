/* Add foreign key constraints to table "DAILY_PERCENTAGES_DEFAULTS".                         */
alter table "DAILY_PERCENTAGES_DEFAULTS"
	add constraint "HIER_NODE_DLY_PCS_DFLTS_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
