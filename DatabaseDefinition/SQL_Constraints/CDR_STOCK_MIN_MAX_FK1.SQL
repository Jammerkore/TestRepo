alter table "STOCK_MIN_MAX"
	add constraint "CDR_STOCK_MIN_MAX_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
