alter table "HEADER_BULK_COLOR_SIZE_NEED"
	add constraint "SZ_ALT_M_HDR_BC_SZ_ND_FK1" foreign key (
		"SIZE_ALTERNATE_RID")
	 references "SIZE_ALTERNATE_MODEL" (
		"SIZE_ALTERNATE_RID")  
go
