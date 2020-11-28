/* Add foreign key constraints to table "TASK_FORECAST_DETAIL".                               */
alter table "TASK_FORECAST_DETAIL"
	add constraint "TASK_FOR_TASK_FOR_DETL_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID",
		"FORECAST_SEQUENCE")
	 references "TASK_FORECAST" (
		"TASK_SEQUENCE",
		"TASKLIST_RID",
		"FORECAST_SEQUENCE")  

go
