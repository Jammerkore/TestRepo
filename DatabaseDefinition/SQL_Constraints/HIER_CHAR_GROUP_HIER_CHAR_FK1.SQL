/* Add foreign key constraints to table "HIER_CHAR".                                          */
alter table "HIER_CHAR"
	add constraint "HIER_CHAR_GROUP_HIER_CHAR_FK1" foreign key (
		"HCG_RID")
	 references "HIER_CHAR_GROUP" (
		"HCG_RID")  

go
