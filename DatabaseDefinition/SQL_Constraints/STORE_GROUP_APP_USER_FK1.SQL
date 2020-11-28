/* Add foreign key constraints to table "STORE_GROUP".                                  */
alter table "STORE_GROUP"
	add constraint "STORE_GROUP_APP_USER_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
