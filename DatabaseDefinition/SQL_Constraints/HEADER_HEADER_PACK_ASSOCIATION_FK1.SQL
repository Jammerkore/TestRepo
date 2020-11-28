/* Add foreign key constraints to table "HEADER_PACK_ASSOCIATION".                                        */
alter table "HEADER_PACK_ASSOCIATION"
	add constraint "HEADER_HEADER_PACK_ASSOCIATION_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
