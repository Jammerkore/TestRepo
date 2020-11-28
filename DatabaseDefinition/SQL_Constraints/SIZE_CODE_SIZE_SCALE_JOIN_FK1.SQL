alter table "SIZE_CURVE_JOIN"
	add constraint "SIZE_CODE_SIZE_SCALE_JOIN_FK1" foreign key (
		"SIZE_CODE_RID")
	 references "SIZE_CODE" (
		"SIZE_CODE_RID")  

go
