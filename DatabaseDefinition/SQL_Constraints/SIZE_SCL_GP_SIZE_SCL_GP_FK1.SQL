/* Add foreign key constraints to table "SIZE_CURVE_GROUP_JOIN".                              */
alter table "SIZE_CURVE_GROUP_JOIN"
	add constraint "SIZE_SCL_GP_SIZE_SCL_GP_FK1" foreign key (
		"SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" (
		"SIZE_CURVE_GROUP_RID")  

go
