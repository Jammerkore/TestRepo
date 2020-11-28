/* Add foreign key constraints to table "SIZE_CURVE_JOIN".                                    */
alter table "SIZE_CURVE_JOIN"
	add constraint "SIZE_SCALE_SIZE_SCALE_JOIN_FK1" foreign key (
		"SIZE_CURVE_RID")
	 references "SIZE_CURVE" (
		"SIZE_CURVE_RID")  

go
