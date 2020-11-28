alter table "GROUP_LEVEL_BASIS"
	add constraint "HIERARCHY_NODE_GROUP_LEVEL_BASIS_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
