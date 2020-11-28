alter table "HEADER_BULK_COLOR_SIZE_NEED"
	add constraint "SZ_CON_M_HDR_BC_SZ_ND_FK1" foreign key (
		"SIZE_CONSTRAINT_RID")
	 references "SIZE_CONSTRAINT_MODEL" (
		"SIZE_CONSTRAINT_RID")  
     
go
