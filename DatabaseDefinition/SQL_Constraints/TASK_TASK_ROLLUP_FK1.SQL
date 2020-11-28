/* Add foreign key constraints to table "TASK_ROLLUP".                                        */
alter table "TASK_ROLLUP"
	add constraint "TASK_TASK_ROLLUP_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID")
	 references "TASK" (
		"TASK_SEQUENCE",
		"TASKLIST_RID")  

go
