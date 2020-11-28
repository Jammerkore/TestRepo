/* Add foreign key constraints to table "TASK".                                               */
alter table "TASK"
	add constraint "TASKLIST_TASK_FK1" foreign key (
		"TASKLIST_RID")
	 references "TASKLIST" (
		"TASKLIST_RID")  

go
