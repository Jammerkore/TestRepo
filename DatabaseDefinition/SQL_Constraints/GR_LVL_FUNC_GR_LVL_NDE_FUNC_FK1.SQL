/* Add foreign key constraints to table "GROUP_LEVEL_NODE_FUNCTION".                          */
alter table "GROUP_LEVEL_NODE_FUNCTION"
	add constraint "GR_LVL_FUNC_GR_LVL_NDE_FUNC_FK1" foreign key (
		"METHOD_RID",
		"SGL_RID")
	 references "GROUP_LEVEL_FUNCTION" (
		"METHOD_RID",
		"SGL_RID")  

go
