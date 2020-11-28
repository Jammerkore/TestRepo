/* Add foreign key constraints to table "STORE_GROUP_LEVEL".                                  */
alter table "STORE_GROUP_LEVEL"
	add constraint "STORE_GROUPING_STORE_GROUP_FK1" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
