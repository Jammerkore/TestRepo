alter table "TOTAL_ALLOCATION"
	add constraint "HEADER_TOTAL_ALLOCATION_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
