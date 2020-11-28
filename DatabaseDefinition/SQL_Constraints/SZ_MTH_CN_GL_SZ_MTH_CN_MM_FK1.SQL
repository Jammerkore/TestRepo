/* Add foreign key constraints to table "SIZE_MTH_RULE_COLOR_SIZE".                           */
alter table "SIZE_MTH_RULE_COLOR_SIZE"
	add constraint "SZ_MTH_CN_GL_SZ_MTH_CN_MM_FK1" foreign key (
		"METHOD_RID",
		"SGL_RID")
	 references "SIZE_MTH_RULE_GRPLVL" (
		"METHOD_RID",
		"SGL_RID")  

go
