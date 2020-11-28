alter table "HEADER_STORE_GRADE_VALUES"  WITH CHECK
	add constraint "STORE_GRP_LVL_HDR_GRADE_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  
go
