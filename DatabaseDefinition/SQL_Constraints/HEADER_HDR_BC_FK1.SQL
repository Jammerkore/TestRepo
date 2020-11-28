alter table "HEADER_BULK_COLOR"
	add constraint "HEADER_HDR_BC_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
