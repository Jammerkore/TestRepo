alter table "BULK_ALLOCATION"
	add constraint "HEADER_BULK_ALLOCATION_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
