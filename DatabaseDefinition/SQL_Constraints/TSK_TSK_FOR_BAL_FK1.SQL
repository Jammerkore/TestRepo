/* Add foreign key constraints to table "TASK_FORECAST_BALANCE".                              */
alter table "TASK_FORECAST_BALANCE"
	add constraint "TSK_TSK_FOR_BAL_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID")
	 references "TASK" (
		"TASK_SEQUENCE",
		"TASKLIST_RID")  

go
