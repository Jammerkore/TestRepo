alter table "TASK_FORECAST_DETAIL"
	add constraint "CAL_DT_RANGE_TASK_FOR_DETL_FK1" foreign key (
		"EXECUTE_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
