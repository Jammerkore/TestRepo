alter table "OTS_PLAN"
	add constraint "CDR_METHOD_OTS_PLAN_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
