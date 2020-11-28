/* Add foreign key constraints to table "TASK_SIZE_CURVE_GENERATE".                           */
alter table "TASK_SIZE_CURVE_GENERATE"
	add constraint "TASK_TASK_SZ_CRV_GEN_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID")
	 references "TASK" (
		"TASK_SEQUENCE",
		"TASKLIST_RID")  

go
