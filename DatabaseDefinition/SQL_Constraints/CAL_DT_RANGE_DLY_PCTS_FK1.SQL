alter table "DAILY_PERCENTAGES"
	add constraint "CAL_DT_RANGE_DLY_PCTS_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
