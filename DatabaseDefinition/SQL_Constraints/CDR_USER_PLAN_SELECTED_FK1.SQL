alter table "USER_PLAN"
	add constraint "CDR_USER_PLAN_SELECTED_FK1" foreign key (
		"TIME_PERIOD_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
