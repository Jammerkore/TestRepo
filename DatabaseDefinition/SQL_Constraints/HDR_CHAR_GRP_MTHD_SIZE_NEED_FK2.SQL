alter table "METHOD_SIZE_NEED"
	add constraint "HDR_CHAR_GRP_MTHD_SIZE_NEED_FK2" foreign key (
		"GENCONSTRAINT_HCG_RID")
	 references "HEADER_CHAR_GROUP" (
		"HCG_RID")  

go
