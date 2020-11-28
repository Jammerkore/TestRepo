alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "HDR_CHAR_GRP_MTHD_FILL_SZ_HLS_FK1" foreign key (
		"GENCURVE_HCG_RID")
	 references "HEADER_CHAR_GROUP" (
		"HCG_RID")  

go
