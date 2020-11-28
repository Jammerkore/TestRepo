alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "SIZE_SCALE_GP_SIZE_FILL_FK1" foreign key (
		"SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" (
		"SIZE_CURVE_GROUP_RID")  

go
