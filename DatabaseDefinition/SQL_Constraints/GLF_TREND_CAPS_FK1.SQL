/* Add foreign key constraints to table "TREND_CAPS".                                         */
alter table "TREND_CAPS"
	add constraint "GLF_TREND_CAPS_FK1" foreign key (
		"METHOD_RID",
		"SGL_RID")
	 references "GROUP_LEVEL_FUNCTION" (
		"METHOD_RID",
		"SGL_RID")  

go
