alter table "METHOD_SIZE_NEED"
	add constraint "SZ_GRP_MTHD_SZ_NEED_FK1" foreign key (
		"SIZE_GROUP_RID")
	 references "SIZE_GROUP" (
		"SIZE_GROUP_RID")  

go
