alter table "METHOD_EXPORT"
	add constraint "CAL_DT_RNG_MTH_EXP_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
