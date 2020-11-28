alter table "BULK_COLOR_ALLOCATION"
	add constraint "HDR_BC_BC_ALOCTN_FK1" foreign key (
		"HDR_BC_RID")
	 references "HEADER_BULK_COLOR" (
		"HDR_BC_RID")  

go
