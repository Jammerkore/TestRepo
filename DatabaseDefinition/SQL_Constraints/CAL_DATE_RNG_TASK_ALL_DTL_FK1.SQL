alter table "TASK_ALLOCATE_DETAIL"
	add constraint "CAL_DATE_RNG_TASK_ALL_DTL_FK1" foreign key (
		"EXECUTE_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
