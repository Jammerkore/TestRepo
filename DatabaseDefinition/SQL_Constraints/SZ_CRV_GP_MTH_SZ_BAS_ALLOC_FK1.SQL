alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "SZ_CRV_GP_MTH_SZ_BAS_ALLOC_FK1" foreign key (
		"SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" (
		"SIZE_CURVE_GROUP_RID")  

go
