/* Add foreign key constraints to table "BULK_COLOR_ALLOCATION".                              */
alter table "BULK_COLOR_ALLOCATION"
	add constraint "HEADER_BC_ALOCTN_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
