alter table "METHOD_GROUP_ALLOCATION"
	add constraint "METHOD_GROUP_ALLOCATION_HN_FK5" foreign key (
		"HDRIB_MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  
go