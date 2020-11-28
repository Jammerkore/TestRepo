alter table "GROUP_LEVEL_FUNCTION"
	add constraint "SGL_METHOD_PCT_CONTRIB_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
