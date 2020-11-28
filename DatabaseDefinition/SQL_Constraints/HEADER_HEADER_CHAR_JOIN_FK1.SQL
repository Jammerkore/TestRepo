/* Add foreign key constraints to table "HEADER_CHAR_JOIN".                                   */
alter table "HEADER_CHAR_JOIN"
	add constraint "HEADER_HEADER_CHAR_JOIN_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
