alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "HDR_CHAR_GRP_MTHD_SZ_BS_ALLOC_FK2" foreign key (
		"GENCONSTRAINT_HCG_RID")
	 references "HEADER_CHAR_GROUP" (
		"HCG_RID")  

go
