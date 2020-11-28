/* Add foreign key constraints to table "TASK_POSTING".                                       */
alter table "TASK_POSTING"
	add constraint "TASK_TASK_POSTING_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID")
	 references "TASK" (
		"TASK_SEQUENCE",
		"TASKLIST_RID")  

go
