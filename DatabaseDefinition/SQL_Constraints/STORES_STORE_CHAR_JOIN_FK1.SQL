/* Add foreign key constraints to table "STORE_CHAR_JOIN".                                    */
alter table "STORE_CHAR_JOIN"
	add constraint "STORES_STORE_CHAR_JOIN_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
