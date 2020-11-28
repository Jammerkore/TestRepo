/* Add foreign key constraints to table "HEADER_BULK_COLOR".                                  */
alter table "HEADER_BULK_COLOR"
	add constraint "COLOR_CODE_HDR_BC_FK1" foreign key (
		"COLOR_CODE_RID")
	 references "COLOR_CODE" (
		"COLOR_CODE_RID")  

go
