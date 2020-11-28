/* Add foreign key constraints to table "TASK_ALLOCATE_DETAIL".                               */
alter table "TASK_ALLOCATE_DETAIL"
	add constraint "TASK_ALL_TASK_ALL_DTL_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID",
		"ALLOCATE_SEQUENCE")
	 references "TASK_ALLOCATE" (
		"TASK_SEQUENCE",
		"TASKLIST_RID",
		"ALLOCATE_SEQUENCE")  

go
