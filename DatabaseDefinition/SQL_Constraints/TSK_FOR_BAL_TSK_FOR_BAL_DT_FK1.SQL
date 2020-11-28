/* Add foreign key constraints to table "TASK_FORECAST_BALANCE_DETAIL".                       */
alter table "TASK_FORECAST_BALANCE_DETAIL"
	add constraint "TSK_FOR_BAL_TSK_FOR_BAL_DT_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID",
		"FORECAST_BALANCE_SEQUENCE")
	 references "TASK_FORECAST_BALANCE" (
		"TASK_SEQUENCE",
		"TASKLIST_RID",
		"FORECAST_BALANCE_SEQUENCE")  

go
