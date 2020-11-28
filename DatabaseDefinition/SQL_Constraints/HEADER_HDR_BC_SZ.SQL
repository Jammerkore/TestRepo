/* Add foreign key constraints to table "HEADER_BULK_COLOR_SIZE".                             */
alter table "HEADER_BULK_COLOR_SIZE"
	add constraint "HEADER_HDR_BC_SZ" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
