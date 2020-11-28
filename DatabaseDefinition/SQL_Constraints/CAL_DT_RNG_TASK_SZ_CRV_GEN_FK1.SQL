alter table "TASK_SIZE_CURVE_GENERATE"
	add constraint "CAL_DT_RNG_TASK_SZ_CRV_GEN_FK1" foreign key (
		"EXECUTE_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
