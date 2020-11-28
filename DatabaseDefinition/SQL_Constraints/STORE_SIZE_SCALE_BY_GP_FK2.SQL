alter table "STORE_SIZE_CURVE_BY_GROUP"
	add constraint "STORE_SIZE_SCALE_BY_GP_FK2" foreign key (
		"SIZE_CURVE_RID")
	 references "SIZE_CURVE" (
		"SIZE_CURVE_RID")  

go
