/* Add foreign key constraints to table "TASK_ALLOCATE".                                      */
alter table "TASK_ALLOCATE"
	add constraint "TASK_TASK_ALLOCATE_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID")
	 references "TASK" (
		"TASK_SEQUENCE",
		"TASKLIST_RID")  

go
