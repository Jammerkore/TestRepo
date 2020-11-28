alter table "USER_PLAN_BASIS_DETAILS"
	add constraint "CDRU_PLAN_BASIS_DETAILS_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
