alter table "HEADER"  WITH CHECK
	add constraint "STORE_GROUP_HEADER_FK1" foreign key (
		"GRADE_SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  
go
