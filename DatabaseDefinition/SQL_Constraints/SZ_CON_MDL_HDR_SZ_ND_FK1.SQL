alter table "HEADER_SIZE_NEED"
	add constraint "SZ_CON_MDL_HDR_SZ_ND_FK1" foreign key (
		"SIZE_CONSTRAINT_RID")
	 references "SIZE_CONSTRAINT_MODEL" (
		"SIZE_CONSTRAINT_RID")  

go
