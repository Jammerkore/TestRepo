alter table "USER_ALLOCATION"
	add constraint "APP_USER_USER_ALLOC_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
