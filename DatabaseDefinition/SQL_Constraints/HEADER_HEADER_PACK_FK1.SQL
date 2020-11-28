/* Add foreign key constraints to table "HEADER_PACK".                                        */
alter table "HEADER_PACK"
	add constraint "HEADER_HEADER_PACK_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
