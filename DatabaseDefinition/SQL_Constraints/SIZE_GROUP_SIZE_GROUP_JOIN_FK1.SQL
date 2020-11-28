/* Add foreign key constraints to table "SIZE_GROUP_JOIN".                                    */
alter table "SIZE_GROUP_JOIN"
	add constraint "SIZE_GROUP_SIZE_GROUP_JOIN_FK1" foreign key (
		"SIZE_GROUP_RID")
	 references "SIZE_GROUP" (
		"SIZE_GROUP_RID")  

go
