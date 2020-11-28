/* Add foreign key constraints to table "USER_GROUP_JOIN".                                    */
alter table "USER_GROUP_JOIN"
	add constraint "USER_GROUP_USER_GROUP_JOIN_FK1" foreign key (
		"GROUP_RID")
	 references "USER_GROUP" (
		"GROUP_RID")  

go
