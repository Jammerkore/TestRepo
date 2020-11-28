alter table "SIZE_CURVE_GROUP"
	add constraint "SIZE_SCALE_SIZE_SCALE_GP_FK1" foreign key (
		"DEFAULT_SIZE_CURVE_RID")
	 references "SIZE_CURVE" (
		"SIZE_CURVE_RID")  

go
