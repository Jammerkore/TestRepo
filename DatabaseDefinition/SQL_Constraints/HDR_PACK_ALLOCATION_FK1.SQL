/* Add foreign key constraints to table "PACK_ALLOCATION".                                    */
alter table "PACK_ALLOCATION"
	add constraint "HDR_PACK_ALLOCATION_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
