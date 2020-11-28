alter table "HIER_CHAR_GROUP_JOIN"
	add constraint "HIER_CHAR_GROUP_JOIN_FK1" foreign key (
		"HCG_RID")
	 references "HIER_CHAR_GROUP" (
		"HCG_RID")  

go
