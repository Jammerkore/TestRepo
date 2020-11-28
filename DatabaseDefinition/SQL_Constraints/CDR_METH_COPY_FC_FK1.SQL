alter table "METHOD_COPY_FORECAST"
	add constraint "CDR_METH_COPY_FC_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
