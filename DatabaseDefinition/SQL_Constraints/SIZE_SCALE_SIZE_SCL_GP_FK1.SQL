alter table "SIZE_CURVE_GROUP_JOIN"
	add constraint "SIZE_SCALE_SIZE_SCL_GP_FK1" foreign key (
		"SIZE_CURVE_RID")
	 references "SIZE_CURVE" (
		"SIZE_CURVE_RID")  

go
