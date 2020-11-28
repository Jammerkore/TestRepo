/* Add foreign key constraints to table "SECURITY_USER_STORE".                                */
alter table "SECURITY_USER_STORE"
	add constraint "USER_SECURITY_USER_STORE_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
