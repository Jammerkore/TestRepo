alter table "METHOD_GROUP_ALLOCATION"
	add constraint "METHOD_GROUP_ALLOCATION_HN_FK3" foreign key (
		"ON_HAND_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go