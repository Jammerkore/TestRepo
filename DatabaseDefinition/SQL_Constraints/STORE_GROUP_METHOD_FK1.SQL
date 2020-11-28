/* Add foreign key constraints to table "METHOD".                                             */
alter table "METHOD"
	add constraint "STORE_GROUP_METHOD_FK1" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
