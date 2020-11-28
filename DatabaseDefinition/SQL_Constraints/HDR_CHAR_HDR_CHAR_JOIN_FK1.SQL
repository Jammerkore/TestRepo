alter table "HEADER_CHAR_JOIN"
	add constraint "HDR_CHAR_HDR_CHAR_JOIN_FK1" foreign key (
		"HC_RID")
	 references "HEADER_CHAR" (
		"HC_RID")  

go
