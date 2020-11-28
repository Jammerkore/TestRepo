alter table "HEADER_SIZE_NEED"
	add constraint "SZ_CV_GRP_HDR_SZ_ND_FK1" foreign key (
		"SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" (
		"SIZE_CURVE_GROUP_RID")  

go
