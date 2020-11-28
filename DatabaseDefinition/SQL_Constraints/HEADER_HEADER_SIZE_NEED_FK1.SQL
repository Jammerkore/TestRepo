/* Add foreign key constraints to table "HEADER_SIZE_NEED".                                   */
alter table "HEADER_SIZE_NEED"
	add constraint "HEADER_HEADER_SIZE_NEED_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
