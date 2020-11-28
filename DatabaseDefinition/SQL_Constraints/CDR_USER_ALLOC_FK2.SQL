alter table "USER_ALLOCATION"
	add constraint "CDR_USER_ALLOC_FK2" foreign key (
		"END_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
