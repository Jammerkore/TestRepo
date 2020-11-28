alter table "MASTER_HEADER"
	add constraint "HEADER_PACK_MASTER_HEADER_FK2" foreign key (
		"SUBORD_PACK_RID")
	 references "HEADER_PACK" (
		"HDR_PACK_RID")  

go
