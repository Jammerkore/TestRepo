alter table "HIERARCHY_NODE"
	add constraint "STR_GRP_HIER_NODE_FK1" foreign key (
		"STOCK_MIN_MAX_SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
