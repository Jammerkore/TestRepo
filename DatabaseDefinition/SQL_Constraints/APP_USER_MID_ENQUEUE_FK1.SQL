alter table "MID_ENQUEUE"
	add constraint "APP_USER_MID_ENQUEUE_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
