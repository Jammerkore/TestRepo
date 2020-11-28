alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "SZ_GRP_MTHD_SZ_BAS_ALLOC_FK1" foreign key (
		"SIZE_GROUP_RID")
	 references "SIZE_GROUP" (
		"SIZE_GROUP_RID")  

go
