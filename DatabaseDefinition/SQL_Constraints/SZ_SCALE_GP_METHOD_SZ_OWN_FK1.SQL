alter table "METHOD_SIZE_NEED_ALLOCATION"
	add constraint "SZ_SCALE_GP_METHOD_SZ_OWN_FK1" foreign key (
		"SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" (
		"SIZE_CURVE_GROUP_RID")  

go
