alter table "USER_METHOD_PREF"
	add constraint "APP_USER_METHOD_PREF_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
