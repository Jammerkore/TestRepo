alter table "SECURITY_GROUP_HIERARCHY_NODE"
	add constraint "GROUP_SECURITY_GROUP_HN_FK1" foreign key (
		"GROUP_RID")
	 references "USER_GROUP" (
		"GROUP_RID")  

go
