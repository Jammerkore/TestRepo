alter table "STORE_ELIGIBILITY"
	add constraint "CDR_STORE_ELIGIBILITY_FK1" foreign key (
		"UNTIL_DATE")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
