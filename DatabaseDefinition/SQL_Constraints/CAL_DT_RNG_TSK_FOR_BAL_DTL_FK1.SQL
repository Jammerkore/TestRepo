alter table "TASK_FORECAST_BALANCE_DETAIL"
	add constraint "CAL_DT_RNG_TSK_FOR_BAL_DTL_FK1" foreign key (
		"EXECUTE_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
