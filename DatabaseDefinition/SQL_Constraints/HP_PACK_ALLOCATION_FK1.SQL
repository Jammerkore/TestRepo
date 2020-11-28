alter table "PACK_ALLOCATION"
	add constraint "HP_PACK_ALLOCATION_FK1" foreign key (
		"HDR_PACK_RID")
	 references "HEADER_PACK" (
		"HDR_PACK_RID")  

go
