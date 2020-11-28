/* Add foreign key constraints to table "HEADER_PACK_ASSOCIATION".                                        */
alter table "HEADER_PACK_ASSOCIATION"
	add constraint "HEADER_PACK_HEADER_PACK_ASSOCIATION_FK2" foreign key (
		"ASSOCIATED_PACK_RID")
	 references "HEADER_PACK" (
		"HDR_PACK_RID")  

go
