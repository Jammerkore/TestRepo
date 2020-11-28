/* Add foreign key constraints to table "USER_SESSION".                                       */
alter table "USER_SESSION"
	add constraint "AP_USER_USER_SESSION_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
