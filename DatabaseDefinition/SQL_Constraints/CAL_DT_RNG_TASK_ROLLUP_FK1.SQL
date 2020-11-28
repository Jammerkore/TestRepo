alter table "TASK_ROLLUP"
	add constraint "CAL_DT_RNG_TASK_ROLLUP_FK1" foreign key (
		"ROLLUP_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
