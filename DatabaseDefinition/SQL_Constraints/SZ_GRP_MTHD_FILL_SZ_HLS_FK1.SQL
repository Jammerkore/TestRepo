alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "SZ_GRP_MTHD_FILL_SZ_HLS_FK1" foreign key (
		"SIZE_GROUP_RID")
	 references "SIZE_GROUP" (
		"SIZE_GROUP_RID")  

go
