alter table "HEADER_PACK"
	add constraint "HEADER_PACK_HEADER_PACK_FK1" foreign key (
		"ASSOCIATED_PACK_RID")
	 references "HEADER_PACK" (
		"HDR_PACK_RID")  

go
