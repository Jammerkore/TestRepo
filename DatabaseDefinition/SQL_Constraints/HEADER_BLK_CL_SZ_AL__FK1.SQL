alter table "BULK_COLOR_SIZE_ALLOCATION"
	add constraint "HEADER_BLK_CL_SZ_AL__FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
