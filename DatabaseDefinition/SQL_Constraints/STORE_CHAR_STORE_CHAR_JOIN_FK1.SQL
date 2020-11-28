alter table "STORE_CHAR_JOIN"
	add constraint "STORE_CHAR_STORE_CHAR_JOIN_FK1" foreign key (
		"SC_RID")
	 references "STORE_CHAR" (
		"SC_RID")  ON DELETE CASCADE

go
