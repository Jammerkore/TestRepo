/* Add foreign key constraints to table "HEADER_PACK_COLOR".                                  */
alter table "HEADER_PACK_COLOR"
	add constraint "HP_COLOR_FK1" foreign key (
		"HDR_PACK_RID")
	 references "HEADER_PACK" (
		"HDR_PACK_RID")  

go
