alter table "BULK_COLOR_SIZE_ALLOCATION"
	add constraint "HDR_BLK_CL_SZ_BLK_CL_SZ_AL_FK1" foreign key (
		"HDR_BC_RID",
		"HDR_BCSZ_KEY")
	 references "HEADER_BULK_COLOR_SIZE" (
		"HDR_BC_RID",
		"HDR_BCSZ_KEY")  

go
