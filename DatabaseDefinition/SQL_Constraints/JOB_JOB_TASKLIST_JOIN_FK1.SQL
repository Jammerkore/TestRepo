/* Add foreign key constraints to table "JOB_TASKLIST_JOIN".                                  */
alter table "JOB_TASKLIST_JOIN"
	add constraint "JOB_JOB_TASKLIST_JOIN_FK1" foreign key (
		"JOB_RID")
	 references "JOB" (
		"JOB_RID")  

go
