/* Add foreign key constraints to table "SCHEDULE_JOB_JOIN".                                  */
alter table "SCHEDULE_JOB_JOIN"
	add constraint "SCHEDULE_SCHEDULE_JOB_JOIN_FK1" foreign key (
		"SCHED_RID")
	 references "SCHEDULE" (
		"SCHED_RID")  

go
