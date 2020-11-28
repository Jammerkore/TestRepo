alter table "MASTER_HEADER"
	add constraint "HEADER_BULK_COLOR_MASTER_HEADER_FK2" foreign key (
		"SUBORD_BC_RID")
	 references "HEADER_BULK_COLOR" (
		"HDR_BC_RID")  

go
