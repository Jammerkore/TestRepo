alter table "HEADER_PACK_COLOR"
	add constraint "CCH_PACK_COLOR_FK1" foreign key (
		"COLOR_CODE_RID")
	 references "COLOR_CODE" (
		"COLOR_CODE_RID")  

go
