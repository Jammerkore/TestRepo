/* Add foreign key constraints to table "TASK_HEADER_RECONCILE".                                       */
alter table "TASK_HEADER_RECONCILE"
	add constraint "TASK_TASK_HEADER_RECONCILE_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID")
	 references "TASK" (
		"TASK_SEQUENCE",
		"TASKLIST_RID")  

go
