alter table "HEADER_BULK_COLOR"
	add constraint "HDR_BC_HDR_BC_FK1" foreign key (
		"ASRT_BC_RID")
	 references "HEADER_BULK_COLOR" (
		"HDR_BC_RID")  

go
