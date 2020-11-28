alter table "SCHEDULE_JOB_JOIN"
	add constraint "APP_USER_SCHD_JOB_JOIN_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
