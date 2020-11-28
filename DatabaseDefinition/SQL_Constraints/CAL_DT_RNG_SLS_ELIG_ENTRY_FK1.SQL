alter table "SALES_ELIGIBILITY_ENTRY"
	add constraint "CAL_DT_RNG_SLS_ELIG_ENTRY_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
