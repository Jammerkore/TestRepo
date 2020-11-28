alter table "METHOD_GROUP_ALLOCATION"
	add constraint "METHOD_GROUP_ALLOCATION_HN_FK2" foreign key (
		"MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go