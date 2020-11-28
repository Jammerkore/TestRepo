alter table "METHOD_MOD_SALES"
	add constraint "CALENDAR_DATE_RANGE_METHOD_MOD_SALES_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
