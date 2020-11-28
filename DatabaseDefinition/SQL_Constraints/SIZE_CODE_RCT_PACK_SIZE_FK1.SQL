alter table "HEADER_PACK_COLOR_SIZE"
	add constraint "SIZE_CODE_RCT_PACK_SIZE_FK1" foreign key (
		"SIZE_CODE_RID")
	 references "SIZE_CODE" (
		"SIZE_CODE_RID")  

go
