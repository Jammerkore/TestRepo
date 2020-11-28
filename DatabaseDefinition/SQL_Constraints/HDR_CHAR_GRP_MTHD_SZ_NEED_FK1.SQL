alter table "METHOD_SIZE_NEED"
	add constraint "HDR_CHAR_GRP_MTHD_SZ_NEED_FK1" foreign key (
		"GENCURVE_HCG_RID")
	 references "HEADER_CHAR_GROUP" (
		"HCG_RID")  

go
