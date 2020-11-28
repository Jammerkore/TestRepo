alter table "METHOD_SIZE_NEED_ALLOCATION"
	add constraint "CDR_METHOD_SIZE_NEED_FK1" foreign key (
		"BEGIN_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
