/* Add foreign key constraints to table "TASK_FORECAST".                                      */
alter table "TASK_FORECAST"
	add constraint "TASK_TASK_FORECAST_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID")
	 references "TASK" (
		"TASK_SEQUENCE",
		"TASKLIST_RID")  

go
