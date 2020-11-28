alter table "METHOD_SIZE_NEED"
	add constraint "SZ_CRV_GRP_MTHD_SZ_NEED_FK1" foreign key (
		"SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" (
		"SIZE_CURVE_GROUP_RID")  

go
