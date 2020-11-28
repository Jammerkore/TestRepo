/* Add foreign key constraints to table "SECURITY_GROUP_STORE".                               */
alter table "SECURITY_GROUP_STORE"
	add constraint "GROUP_SECURITY_GROUP_STORE_FK1" foreign key (
		"GROUP_RID")
	 references "USER_GROUP" (
		"GROUP_RID")  

go
