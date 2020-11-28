/* Add foreign key constraints to table "STORE_CHAR".                                         */
alter table "STORE_CHAR"
	add constraint "STORE_CHAR_GROUP_FK1" foreign key (
		"SCG_RID")
	 references "STORE_CHAR_GROUP" (
		"SCG_RID")  

go
