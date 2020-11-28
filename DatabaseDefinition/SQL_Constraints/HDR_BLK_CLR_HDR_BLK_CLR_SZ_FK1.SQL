alter table "HEADER_BULK_COLOR_SIZE"
	add constraint "HDR_BLK_CLR_HDR_BLK_CLR_SZ_FK1" foreign key (
		"HDR_BC_RID")
	 references "HEADER_BULK_COLOR" (
		"HDR_BC_RID")  

go
