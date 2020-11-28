alter table "SCHEDULE_JOB_JOIN"
	add constraint "JOB_SCHEDULE_JOB_JOIN_FK1" foreign key (
		"JOB_RID")
	 references "JOB" (
		"JOB_RID")  

go
