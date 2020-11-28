alter table "SECURITY_USER_VERSION"
	add constraint "USER_SECURITY_USER_VERSION_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
