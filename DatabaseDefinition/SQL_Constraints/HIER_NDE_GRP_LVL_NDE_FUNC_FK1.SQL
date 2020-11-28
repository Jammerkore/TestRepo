alter table "GROUP_LEVEL_NODE_FUNCTION"
	add constraint "HIER_NDE_GRP_LVL_NDE_FUNC_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
