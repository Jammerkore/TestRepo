/* Add foreign key constraints to table "GROUP_LEVEL_BASIS".                                  */
alter table "GROUP_LEVEL_BASIS"
	add constraint "GROUP_LEVEL_FUNCTION_GROUP_LEVEL_BASIS_FK1" foreign key (
		"METHOD_RID",
		"SGL_RID")
	 references "GROUP_LEVEL_FUNCTION" (
		"METHOD_RID",
		"SGL_RID")  

go
