/* Add foreign key constraints to table "SELL_THRU".                                          */
alter table "SELL_THRU"
	add constraint "HIERARCHY_NODE_SELL_THRU_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
