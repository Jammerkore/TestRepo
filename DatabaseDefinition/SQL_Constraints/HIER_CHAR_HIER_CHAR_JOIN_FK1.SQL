alter table "HIER_CHAR_JOIN"
	add constraint "HIER_CHAR_HIER_CHAR_JOIN_FK1" foreign key (
		"HC_RID")
	 references "HIER_CHAR" (
		"HC_RID")  

go
