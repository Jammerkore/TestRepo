/* Add foreign key constraints to table "TASK_PROGRAM".                                       */
alter table "TASK_PROGRAM"
	add constraint "TASK_TASK_PROGRAM_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID")
	 references "TASK" (
		"TASK_SEQUENCE",
		"TASKLIST_RID")  

go
