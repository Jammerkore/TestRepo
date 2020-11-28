alter table "METHOD_SPREAD_FORECAST"
	add constraint "CAL_DT_RNG_MTHD_SPRD_FOR_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
