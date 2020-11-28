/* Add foreign key constraints to table "STORE_SIZE_CURVE_BY_GROUP".                          */
alter table "STORE_SIZE_CURVE_BY_GROUP"
	add constraint "STORE_SIZE_SCALE_BY_GP_FK1" foreign key (
		"SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" (
		"SIZE_CURVE_GROUP_RID")  

go
