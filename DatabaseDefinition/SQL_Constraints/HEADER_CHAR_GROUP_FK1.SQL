/* Add foreign key constraints to table "HEADER_CHAR".                                        */
alter table "HEADER_CHAR"
	add constraint "HEADER_CHAR_GROUP_FK1" foreign key (
		"HCG_RID")
	 references "HEADER_CHAR_GROUP" (
		"HCG_RID")  

go
