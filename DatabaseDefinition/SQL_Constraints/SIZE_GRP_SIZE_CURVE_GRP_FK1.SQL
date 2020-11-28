alter table "SIZE_CURVE_GROUP"
	add constraint "SIZE_GRP_SIZE_CURVE_GRP_FK1" foreign key (
		"DEFINED_SIZE_GROUP_RID")
	 references "SIZE_GROUP" (
		"SIZE_GROUP_RID")  

go
